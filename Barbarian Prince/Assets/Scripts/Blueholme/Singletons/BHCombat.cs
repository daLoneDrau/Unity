using Assets.Scripts.Blueholme.Flyweights;
using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Pooled;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Blueholme.Singletons
{
    public class BHCombat : Combat
    {
        public static void Init()
        {
            if (Instance == null)
            {
                GameObject go = new GameObject
                {
                    name = "BHCombat"
                };
                Instance = go.AddComponent<BHCombat>();
            }
        }
        public void InitiateCombat()
        {
            Script.Instance.SetGlobalVariable("FIGHTING", 1);
        }
        public override float ComputeDamages(BaseInteractiveObject srcIo, BaseInteractiveObject wpnIo, BaseInteractiveObject targetIo, int result)
        {
            float damages = 0;
            // send event to target. someone attacked you!
            Script.Instance.EventSender = srcIo;
            Script.Instance.SendIOScriptEvent(targetIo, ScriptConsts.SM_057_AGGRESSION, null, null);
            if (srcIo != null
                    && targetIo != null)
            {
                if (!targetIo.HasIOFlag(IoGlobals.IO_01_PC)
                        && !targetIo.HasIOFlag(IoGlobals.IO_03_NPC)
                /* && targetIo.HasIOFlag(IoGlobals.fix) */) // ATTACKING AN OBJECT, NOT PC OR NPC
                {
                    if (srcIo.HasIOFlag(IoGlobals.IO_01_PC))
                    {
                        // TODO - will this have repairing?
                        // player fixing the target object
                        // ARX_DAMAGES_DamageFIX(targetIo, player.Full_damages, 0,
                        // 0);
                    }
                    else if (srcIo.HasIOFlag(IoGlobals.IO_03_NPC))
                    {
                        // IONpcData fixing target
                        // ARX_DAMAGES_DamageFIX(targetIo,
                        // io_source->_npcdata->damages, GetInterNum(io_source), 0);
                    }
                    else
                    {
                        // unknown fixing target
                        // ARX_DAMAGES_DamageFIX(targetIo, 1,
                        // GetInterNum(io_source), 0);
                    }
                }
                else // ATTACKING A PC OR NPC
                {
                    float attack, ac;
                    float backstab = 1f;
                    // weapon material
                    String wmat = GetWeaponMaterial(srcIo);
                    // armor material
                    String amat = "FLESH";
                    bool critical = false;

                    if (srcIo.HasIOFlag(IoGlobals.IO_01_PC)) // ATTACKER IS A PC
                    {
                        BHCharacter pc = (BHCharacter)srcIo.PcData;
                        // CHECK FOR CRITICAL
                        if (pc.CalculateCriticalHit()
                                && Script.Instance.SendIOScriptEvent(srcIo, ScriptConsts.SM_054_CRITICAL, null, null) != ScriptConsts.REFUSE)
                        {
                            critical = true;
                        }
                        int wpnId = srcIo.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON);
                        if (Interactive.Instance.HasIO(wpnId))
                        {
                            BaseInteractiveObject io = (BaseInteractiveObject)Interactive.Instance.GetIO(wpnId);
                            // CHECK FOR BACKSTAB
                            if (srcIo.PcData.CalculateBackstab()
                                    && Script.Instance.SendIOScriptEvent(srcIo, ScriptConsts.SM_056_BACKSTAB, null, null) != ScriptConsts.REFUSE)
                            {
                                backstab = io.ItemData.GetBackstabModifier();
                            }
                            // GET DAMAGE
                            damages = GetDamages((BHItemData)io.ItemData, result);
                        }
                    }
                    else // ATTACKER IS A NPC
                    {
                        /*
                        if (srcIo.HasIOFlag(IoGlobals.IO_03_NPC))
                        {
                            int wpnId = srcIo.NpcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON);
                            if (Interactive.Instance.HasIO(wpnId))
                            {
                                BaseInteractiveObject io = (BaseInteractiveObject)Interactive.Instance.GetIO(wpnId);
                                if (io.Weaponmaterial != null
                                        && io.Weaponmaterial.Length > 0)
                                {
                                    wmat = io.Weaponmaterial;
                                }
                                io = null;
                            }
                            else
                            {
                                if (srcIo.Weaponmaterial != null
                                        && srcIo.Weaponmaterial.Length > 0)
                                {
                                    wmat = srcIo.Weaponmaterial;
                                }
                            }
                            attack = srcIo.NpcData.GetFullDamage();
                            if (srcIo.NpcData.CalculateCriticalHit()
                                    && Script.Instance.SendIOScriptEvent(srcIo, ScriptConsts.SM_054_CRITICAL, null, null) != ScriptConsts.REFUSE)
                            {
                                critical = true;
                            }
                            damages = attack * dmgModifier;
                            if (srcIo.NpcData.CalculateBackstab()
                                    && Script.Instance.SendIOScriptEvent(srcIo, ScriptConsts.SM_056_BACKSTAB, null, null) != ScriptConsts.REFUSE)
                            {
                                backstab = this.GetBackstabModifier();
                            }
                        }
                        else
                        {
                            throw new RPGException(ErrorMessage.BAD_PARAMETERS, "Compute Damages call made by non-character");
                        }
                        */
                    }
                    // calculate how much damage is absorbed by armor
                    float absorb = CalculateArmorDeflection(targetIo, result);
                    damages -= absorb;
                    if (damages < 0)
                    {
                        damages = 0;
                    }
                    // TODO - apply effects based on strike area
                    // if (targetIo == inter.iobj[0]) {
                    // ac = player.Full_armor_class;
                    // absorb = player.Full_Skill_Defense * DIV2;
                    // } else {
                    // ac = ARX_INTERACTIVE_GetArmorClass(targetIo);
                    // absorb = targetIo->_npcdata->absorb;
                    // long value = ARX_SPELLS_GetSpellOn(targetIo, SPELL_CURSE);
                    // if (value >= 0) {
                    // float modif = (spells[value].caster_level * 0.05f);
                    // ac *= modif;
                    // absorb *= modif;
                    // }
                    // }
                    amat = this.GetArmourMaterial(targetIo);
                    // TODO - handle backstabbing
                    // damages *= backstab;
                    // dmgs -= dmgs * (absorb * DIV100);

                    // TODO - play sound based on the power of the hit
                    if (damages > 0f)
                    {
                        /*
                        if (critical)
                        {
                            damages = this.ApplyCriticalModifier();
                            // dmgs *= 1.5f;
                        }

                        if (targetIo.HasIOFlag(IoGlobals.IO_01_PC))
                        {
                            // TODO - push player when hit
                            // ARX_DAMAGES_SCREEN_SPLATS_Add(&ppos, dmgs);
                            targetIo.PcData.DamagePlayer(damages, 0, srcIo.RefId);
                            // ARX_DAMAGES_DamagePlayerEquipment(dmgs);
                        }
                        else
                        {
                            // TODO - push IONpcData when hit
                            targetIo.NpcData.DamageNPC(damages, srcIo.RefId, false);
                        }
                        */
                    }
                }
            }
            return damages;
        }
        /// <summary>
        /// Calculates the deflection value for a piece of armour based on the targeted area.
        /// </summary>
        /// <param name="targetIo">the IO being targeted</param>
        /// <param name="targetArea">the area being targeted</param>
        /// <returns>float</returns>
        public float CalculateArmorDeflection(BaseInteractiveObject targetIo, int targetArea)
        {
            float absorb = 0;
            return absorb;
        }
        /// <summary>
        /// Gets the armour's material.  Used for sound effects.
        /// </summary>
        /// <param name="srcIo">the source armour</param>
        /// <returns>string</returns>
        private string GetArmourMaterial(BaseInteractiveObject srcIo)
        {
            string amat = "FLESH";
            if (srcIo.Armormaterial != null
                    && srcIo.Armormaterial.Length > 0)
            {
                amat = srcIo.Armormaterial;
            }
            if (srcIo.HasIOFlag(IoGlobals.IO_03_NPC)
                    || srcIo.HasIOFlag(IoGlobals.IO_01_PC))
            {
                int armrId;
                // TODO - get sound based on strike area
                if (srcIo.HasIOFlag(IoGlobals.IO_03_NPC))
                {
                    armrId = srcIo.NpcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_TORSO);
                }
                else
                {
                    armrId = srcIo.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_TORSO);
                }
                if (Interactive.Instance.HasIO(armrId))
                {
                    BaseInteractiveObject io = (BaseInteractiveObject)Interactive.Instance.GetIO(armrId);
                    if (io.Armormaterial != null
                            && io.Armormaterial.Length > 0)
                    {
                        amat = io.Armormaterial;
                    }
                    io = null;
                }
            }
            return amat;
        }
        /// <summary>
        /// Gets the damage inflicted by the weapon.
        /// </summary>
        /// <param name="item">the weapon data</param>
        /// <param name="strikeResult">the result of the strike</param>
        /// <returns>int</returns>
        private int GetDamages(BHItemData item, int strikeResult)
        {
            int damage = item.Dice.Roll();
            damage += item.DmgModifier;
            return damage;
        }
        /// <summary>
        /// Gets all strike modifiers to apply to the strike roll.
        /// </summary>
        /// <param name="io_source">the weapon's wielder</param>
        /// <returns></returns>
        private int GetStrikeModifiers(BaseInteractiveObject srcIo, BaseInteractiveObject wpnIo)
        {
            int mod = 0;
            return mod;
        }
        /// <summary>
        /// Gets the weapon's material.  Used for sound effects.
        /// </summary>
        /// <param name="srcIo">the source weapon</param>
        /// <returns>string</returns>
        private string GetWeaponMaterial(BaseInteractiveObject srcIo)
        {
            // weapon material
            string wmat = "BARE";
            if (srcIo.HasIOFlag(IoGlobals.IO_01_PC)) // attacker is a PC
            {
                int wpnId = srcIo.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON);
                // get the weapon's material for sound effects
                if (Interactive.Instance.HasIO(wpnId))
                {
                    BaseInteractiveObject io = (BaseInteractiveObject)Interactive.Instance.GetIO(wpnId);
                    if (io.Weaponmaterial != null
                            && io.Weaponmaterial.Length > 0)
                    {
                        wmat = io.Weaponmaterial;
                    }
                    io = null;
                }
            }
            else if (srcIo.Weaponmaterial != null
                         && srcIo.Weaponmaterial.Length > 0)
            {
                wmat = srcIo.Weaponmaterial;
            }
            return wmat;
        }
        public override bool StrikeCheck(BaseInteractiveObject srcIo, BaseInteractiveObject wpnIo, long flags, int targ)
        {
            int source = srcIo.RefId;
            int weapon = wpnIo.RefId;
            if (Interactive.Instance.HasIO(source)
                && Interactive.Instance.HasIO(weapon)
                && Interactive.Instance.HasIO(Interactive.Instance.GetIO(targ)))
            {
                BaseInteractiveObject targIo = Interactive.Instance.GetIO((int)targ);
                float dmgs = 0;
                // get specials, such as drain life or paralyze
                /*
                 * float drain_life = ARX_EQUIPMENT_GetSpecialValue(wpnIo, IO_SPECIAL_ELEM_DRAIN_LIFE);
                 * float paralyse = ARX_EQUIPMENT_GetSpecialValue(wpnIo, IO_SPECIAL_ELEM_PARALYZE);
                 */
                // check for a hit
                int roll = Diceroller.Instance.RolldX(6);
                roll += GetStrikeModifiers(srcIo, wpnIo);
                if (roll < -6)
                {
                    roll = -6;
                }
                else if (roll > 14)
                {
                    roll = 14;
                }
                // TODO check for lunge
                int result = StrikeCheck(roll, Diceroller.Instance.RolldX(100));
                // store message about attack
                Messages.Instance.Add(BuildMessage(srcIo, targIo, result, dmgs));
            }
            return false;
        }
        private string BuildMessage(BaseInteractiveObject attacker, BaseInteractiveObject target, int result, float damages = 0f)
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            if (attacker.HasIOFlag(IoGlobals.IO_01_PC))
            {
                sb.Append(attacker.PcData.Name);
            }
            string targName = "";
            if (target.HasIOFlag(IoGlobals.IO_01_PC))
            {
                targName = target.PcData.Name;
            }
            if (result == 0)
            {
                sb.Append(" slashes wildly at ");
                sb.Append(targName);
                sb.Append(" but misses.\n");
            } else
            {
                sb.Append(" hits ");
                sb.Append(targName);
                sb.Append(" for ");
                sb.Append((int)damages);
                sb.Append(" damage!\n");
            }
            string s = sb.ToString();
            sb.ReturnToPool();
            sb = null;
            return s;
        }
        /// <summary>
        /// Checks the Strike Matrix for a hit.
        /// </summary>
        /// <param name="key">the dice roll plus modifiers</param>
        /// <param name="roll">the percentile roll, plus lunge modifier</param>
        /// <returns></returns>
        private int StrikeCheck(int key, int roll)
        {
            int result = 0;
            return result;
        }
    }
}
