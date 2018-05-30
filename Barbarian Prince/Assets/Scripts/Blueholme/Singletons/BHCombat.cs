using Assets.Scripts.Blueholme.Flyweights;
using Assets.Scripts.Blueholme.Globals;
using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Pooled;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

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
        /// <summary>
        /// the list of combatants.
        /// </summary>
        private List<BHInteractiveObject> side1;
        /// <summary>
        /// the list of combatants.
        /// </summary>
        private List<BHInteractiveObject> side2;
        /// <summary>
        /// all combatants who strike in the initial melee.
        /// </summary>
        private List<BHInteractiveObject> initial = new List<BHInteractiveObject>();
        /// <summary>
        /// combatants who get one more strike per round.
        /// </summary>
        private List<BHInteractiveObject> supplemental = new List<BHInteractiveObject>();
        public Text Output { get; set; }
        /// <summary>
        /// The roll needed for a level 0 PC to hit Armour Class 0.
        /// </summary>
        private const int PC_LVL_0_THAC0 = 20;
        /// <summary>
        /// A map of the rolls needed for creatures to hit Armour Class 0.
        /// </summary>
        private static Dictionary<int, int> CREATURE_THAC0 = new Dictionary<int, int>() {
            { 0, 20 },
            { 1, 19 },
            { 2, 18 },
            { 3, 17 },
            { 4, 15 },
            { 5, 14 },
            { 6, 14 },
            { 7, 13 },
            { 8, 13 },
            { 9, 12 },
            { 10, 12 },
            { 11, 11 }
        };
        /// <summary>
        /// The roll needed for a level 1-3 PC to hit Armour Class 0.
        /// </summary>
        private const int PC_LVL_1_3_THAC0 = 19;
        public int Round { get; private set; }
        /// <summary>
        /// Determines if combat is over.  Combat is over when either side has no combatants left.
        /// </summary>
        public bool IsCombatOver
        {
            get
            {
                bool side1dead = true, side2dead = true;
                for (int i = side2.Count - 1; i >= 0; i--)
                {
                    if (!IsIoDead(side2[i]))
                    {
                        side2dead = false;
                        break;
                    }
                }
                for (int i = side1.Count - 1; i >= 0; i--)
                {
                    if (!IsIoDead(side1[i]))
                    {
                        side1dead = false;
                        break;
                    }
                }
                return side1dead || side2dead;
            }
        }
        /// <summary>
        /// Gets the modifier applied to the damage amount for a successful critical hit.
        /// </summary>
        /// <returns><see cref="float"/></returns>
        protected override float ApplyCriticalModifier()
        {
            return 1;
        }
        private string BuildMessage(BaseInteractiveObject attacker, BaseInteractiveObject source, BaseInteractiveObject target, int result, float damages = 0f)
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            int gender = 0;
            if (attacker.HasIOFlag(IoGlobals.IO_01_PC))
            {
                sb.Append(attacker.PcData.Name);
                gender = attacker.PcData.Gender;
            }
            string targName = "";
            if (target.HasIOFlag(IoGlobals.IO_01_PC))
            {
                targName = target.PcData.Name;
            }
            sb.Append(" attacks ");
            sb.Append(targName);
            sb.Append(" with ");
            sb.Append(Gender.GENDER_POSSESSIVE[gender]);
            sb.Append(" ");
            sb.Append(source.ItemData.ItemName);
            sb.Append(" - ");
            if (result == 0)
            {
                sb.Append("MISS!\n");
            }
            else
            {
                sb.Append("HIT for ");
                sb.Append((int)damages);
                sb.Append(" damage!\n");
            }
            string s = sb.ToString();
            sb.ReturnToPool();
            sb = null;
            return s;
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
        private bool IsIoDead(BaseInteractiveObject io)
        {
            bool dead = false;
            if (io.HasIOFlag(IoGlobals.IO_01_PC))
            {
                dead = io.PcData.IsDead();
            }
            return dead;
        }
        public void DoRound()
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append("Round ");
            sb.Append(Round + 1);
            sb.Append("\n");
            Messages.Instance.Add(sb.ToString());
            InitiativeComparer sorter = new InitiativeComparer();
            initial.Clear();
            supplemental.Clear();
            // get IO weapon speeds
            for (int i = side1.Count - 1; i >= 0; i--)
            {
                BHInteractiveObject io = side1[i];
                // TODO - is io stunned? paralyzed?
                int wpnSpeed = BHGlobals.LIGHT_WEAPON;
                if (io.HasIOFlag(IoGlobals.IO_01_PC))
                {
                    wpnSpeed = GetWeaponSpeed(io.PcData);
                }
                if (wpnSpeed == BHGlobals.HEAVY_WEAPON)
                {
                    // heavy weapons strike every other round
                    if (io.Script.GetLocalIntVariableValue("last_round_attack") + 2 == Round)
                    {
                        // IO gets to attack this round
                        initial.Add(io);
                    }
                }
                else
                {
                    // light and medium weapons strike every round
                    initial.Add(io);
                }
                if (wpnSpeed == BHGlobals.LIGHT_WEAPON)
                {
                    // light weapons strike 2x every round
                    supplemental.Add(io);
                }
            }
            for (int i = side2.Count - 1; i >= 0; i--)
            {
                BHInteractiveObject io = side2[i];
                // TODO - is io stunned? paralyzed?
                int wpnSpeed = BHGlobals.LIGHT_WEAPON;
                if (io.HasIOFlag(IoGlobals.IO_01_PC))
                {
                    wpnSpeed = GetWeaponSpeed(io.PcData);
                }
                if (wpnSpeed == BHGlobals.HEAVY_WEAPON)
                {
                    // heavy weapons strike every other round
                    if (io.Script.GetLocalIntVariableValue("last_round_attack") + 2 == Round)
                    {
                        // IO gets to attack this round
                        initial.Add(io);
                    }
                    else
                    {
                        sb.Append(io.PcData.Name);
                        sb.Append(" readies ");
                        sb.Append(Gender.GENDER_POSSESSIVE[io.PcData.Gender]);
                        sb.Append(" weapon.\n");
                        Messages.Instance.Add(sb.ToString());
                    }
                }
                else
                {
                    // light and medium weapons strike every round
                    initial.Add(io);
                }
                if (wpnSpeed == BHGlobals.LIGHT_WEAPON)
                {
                    // light weapons strike 2x every round
                    supplemental.Add(io);
                }
            }
            // sort the combatants
            initial.Sort(sorter);
            supplemental.Sort(sorter);
            // give each combatant an initial attack
            ExecuteCombatantsActions(initial);
            // allow supplemental attacks
            ExecuteCombatantsActions(supplemental);
            Round++;
            sb.ReturnToPool();
        }
        private void ExecuteCombatantsActions(List<BHInteractiveObject> list)
        {
            for (int i = 0, li = list.Count; i < li; i++)
            {
                // get each combatants action
                // for now it will be to strike
                BHInteractiveObject io = list[i];
                // TODO check to see if IO is dead
                if (io.HasIOFlag(IoGlobals.IO_01_PC))
                {
                    Debug.Log(io.PcData.Name);
                    if (IsIoDead(io))
                    {
                        Debug.Log("is dead");
                    }
                    else
                    {
                        BHInteractiveObject wpnIo = null;
                        int wpnId = io.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON);
                        if (Interactive.Instance.HasIO(wpnId))
                        {
                            wpnIo = (BHInteractiveObject)Interactive.Instance.GetIO(wpnId);
                        }
                        // TODO check to see if target is dead
                        int targId = io.Script.GetLocalIntVariableValue("target_practice");
                        if (Interactive.Instance.HasIO(targId)
                            && !IsIoDead(Interactive.Instance.GetIO(targId)))
                        {
                            // IO can strike.  check to see if attack is allowed
                            int lastRoundAttack = io.Script.GetLocalIntVariableValue("last_round_attack");
                            int wpnSpd = GetWeaponSpeed(io.PcData);
                            // HVY weapons strike every other round
                            if (wpnSpd == BHGlobals.HEAVY_WEAPON)
                            {
                                if (Round - lastRoundAttack == 2)
                                {
                                    Debug.Log("`shiudl strike");
                                    StrikeCheck(io, wpnIo, 0, targId);
                                    io.Script.SetLocalVariable("last_round_attack", Round);
                                }
                                else
                                {
                                    Debug.Log("`no strike");
                                    PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                                    sb.Append(io.PcData.Name);
                                    sb.Append(" readies ");
                                    sb.Append(Gender.GENDER_POSSESSIVE[io.PcData.Gender]);
                                    sb.Append(" weapon.\n");
                                    Messages.Instance.Add(sb.ToString());
                                    sb.ReturnToPool();
                                }
                            }
                            else
                            {
                                Debug.Log("`shiudl strike");
                                StrikeCheck(io, wpnIo, 0, targId);
                                io.Script.SetLocalVariable("last_round_attack", Round);
                            }
                            if (IsIoDead(Interactive.Instance.GetIO(targId)))
                            {
                                BaseInteractiveObject targIo = Interactive.Instance.GetIO(targId);
                                PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                                if (targIo.HasIOFlag(IoGlobals.IO_01_PC))
                                {
                                    sb.Append(targIo.PcData.Name);
                                }
                                sb.Append(" dies!\n");
                                Messages.Instance.Add(sb.ToString());
                                sb.ReturnToPool();
                            }
                        }
                    }
                }
                else if (io.HasIOFlag(IoGlobals.IO_03_NPC))
                {

                }
            }
        }
        /// <summary>
        /// Gets the speed of the weapon the PC is wielding
        /// </summary>
        /// <param name="pc">the <see cref="IOPcData"/></param>
        /// <returns></returns>
        private int GetWeaponSpeed(IOPcData pc)
        {
            int wpnSpeed = BHGlobals.LIGHT_WEAPON;
            int wpnId = pc.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON);
            if (Interactive.Instance.HasIO(wpnId))
            {
                BHInteractiveObject wpnIo = (BHInteractiveObject)Interactive.Instance.GetIO(wpnId);
                wpnSpeed = ((BHItemData)wpnIo.ItemData).WeaponSpeed;
            }
            else // PC has no weapons. fighting with fist?
            {

            }
            // Halfling weapon speeds are increased due to their diminutive size
            if (((BHCharacter)pc).Race == BHRace.Halfling)
            {
                wpnSpeed++;
            }
            return wpnSpeed;
        }
        public void InitiateCombat(List<BHInteractiveObject> a, List<BHInteractiveObject> b)
        {
            Script.Instance.SetGlobalVariable("FIGHTING", 1);
            side1 = a;
            for (int i = side1.Count - 1; i >= 0; i--)
            {
                side1[i].Script.SetLocalVariable("last_round_attack", -1);
            }
            side2 = b;
            for (int i = side2.Count - 1; i >= 0; i--)
            {
                side2[i].Script.SetLocalVariable("last_round_attack", -1);
            }
            Round = 0;
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            do
            {
                DoRound();
                sb.Append(Output.text);
                while (!Messages.Instance.IsEmpty)
                {
                    sb.Append(Messages.Instance.Dequeue);
                }
                for (int i = side1.Count - 1; i >= 0; i--)
                {
                    sb.Append(GetStats(side1[i]));
                }
                for (int i = side2.Count - 1; i >= 0; i--)
                {
                    sb.Append(GetStats(side2[i]));
                }
                Output.text = sb.ToString();
                sb.Length = 0;
            } while (!IsCombatOver);
            sb.ReturnToPool();
        }
        private string GetStats(BHInteractiveObject io)
        {
            PooledStringBuilder sb0 = StringBuilderPool.Instance.GetStringBuilder();
            sb0.Append(io.PcData.Name);
            sb0.Append("  ");
            sb0.Append(io.PcData.Life);
            sb0.Append("/");
            sb0.Append(io.PcData.GetMaxLife());
            sb0.Append("\tAC: ");
            sb0.Append(io.PcData.GetFullAttributeScore("AC"));
            sb0.Append("\n");
            string s = sb0.ToString();
            sb0.ReturnToPool();
            return s;
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
                int targAc = 0;
                if (targIo.HasIOFlag(IoGlobals.IO_01_PC))
                {
                    targIo.PcData.ComputeFullStats();
                    targAc = (int)targIo.PcData.GetFullAttributeScore("AC");
                }
                float dmgs = 0;
                // TODO get specials, such as drain life or paralyze
                /*
                 * float drain_life = ARX_EQUIPMENT_GetSpecialValue(wpnIo, IO_SPECIAL_ELEM_DRAIN_LIFE);
                 * float paralyse = ARX_EQUIPMENT_GetSpecialValue(wpnIo, IO_SPECIAL_ELEM_PARALYZE);
                 */
                // check for a hit
                int roll = Diceroller.Instance.RolldX(20);
                if (srcIo.HasIOFlag(IoGlobals.IO_01_PC))
                {
                    srcIo.PcData.ComputeFullStats();
                    roll += (int)srcIo.PcData.GetFullAttributeScore("THM");
                }
                roll += GetStrikeModifiers(srcIo, wpnIo);
                Debug.Log(srcIo.PcData.Name + " attacks AC " + targAc + " - roll is " + roll);
                int result = StrikeCheck(roll, targAc);
                if (result > 0)
                {
                    dmgs = ComputeDamages(srcIo, wpnIo, targIo, result);
                    // damage the target
                    if (targIo.HasIOFlag(IoGlobals.IO_01_PC))
                    {
                        Debug.Log(targIo.PcData.Name + " takes " + dmgs + " damage");
                        targIo.PcData.DamagePlayer(dmgs, 0, srcIo.RefId);
                    }
                    else if (targIo.HasIOFlag(IoGlobals.IO_03_NPC))
                    {
                        targIo.NpcData.DamageNPC(dmgs, srcIo.RefId, false);
                    }
                }
                // store message about attack
                Messages.Instance.Add(BuildMessage(srcIo, wpnIo, targIo, result, dmgs));
            }
            return false;
        }
        /// <summary>
        /// Checks the Strike Matrix for a hit.
        /// </summary>
        /// <param name="key">the dice roll plus modifiers</param>
        /// <param name="roll">the percentile roll, plus lunge modifier</param>
        /// <returns></returns>
        private int StrikeCheck(int roll, int opponentAc)
        {
            int result = 0;
            // check against matrix
            int rollNeeded = PC_LVL_1_3_THAC0;
            rollNeeded -= opponentAc;
            if (roll >= rollNeeded)
            {
                result = 1;
            }
            return result;
        }
        private class InitiativeComparer : IComparer<BHInteractiveObject>
        {
            private float GetIoDex(BHInteractiveObject io)
            {
                float dex = 0;
                if (io.HasIOFlag(IoGlobals.IO_01_PC))
                {
                    io.PcData.ComputeFullStats();
                    dex = io.PcData.GetFullAttributeScore("DEX");
                }
                else if (io.HasIOFlag(IoGlobals.IO_03_NPC))
                {
                    if (io.IsInGroup("HUMANOIDS"))
                    {
                        if (io.Script.HasLocalVariable("DEXTERITY"))
                        {
                            dex = io.Script.GetLocalFloatVariableValue("DEXTERITY");
                        }
                        else
                        {
                            dex = Diceroller.Instance.RollXdY(3, 6);
                            io.Script.SetLocalVariable("DEXTERITY", dex);
                        }
                    }
                }
                return dex;
            }
            public int Compare(BHInteractiveObject x, BHInteractiveObject y)
            {
                int c = 0;
                float xd = GetIoDex(x), yd = GetIoDex(y);
                if (xd < yd)
                {
                    c = 1;
                }
                else if (xd > yd)
                {
                    c = -1;
                }
                else
                {
                    int xr = 0, yr = 0;
                    do
                    {
                        xr = Diceroller.Instance.RolldX(6);
                        yr = Diceroller.Instance.RolldX(6);
                    } while (xr == yr);
                    if (xr < yr)
                    {
                        c = 1;
                    }
                    else
                    {
                        c = -1;
                    }
                }
                return c;
            }
        }
    }
}
