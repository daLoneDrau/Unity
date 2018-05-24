using Assets.Scripts.FantasyWargaming.Flyweights;
using Assets.Scripts.FantasyWargaming.Scriptables.Mobs;
using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Pooled;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.FantasyWargaming.Singletons
{
    public class FWCombat : Combat
    {
        public static void Init()
        {
            if (Instance == null)
            {
                GameObject go = new GameObject
                {
                    name = "FWCombat"
                };
                Instance = go.AddComponent<FWCombat>();
            }
        }
        /// <summary>
        /// Strike check passed with MISS results.
        /// </summary>
        public const int STRIKE_MISS = 0;
        /// <summary>
        /// Strike check passed with SHIELD results.
        /// </summary>
        public const int STRIKE_SHIELD = 1;
        /// <summary>
        /// Strike check passed with TRUNK results.
        /// </summary>
        public const int STRIKE_TRUNK = 2;
        /// <summary>
        /// Strike check passed with FACE results.
        /// </summary>
        public const int STRIKE_FACE = 3;
        /// <summary>
        /// Strike check passed with HEAD results.
        /// </summary>
        public const int STRIKE_HEAD = 4;
        /// <summary>
        /// Strike check passed with SWORD ARM results.
        /// </summary>
        public const int STRIKE_SWORD_ARM = 5;
        /// <summary>
        /// Strike check passed with OTHER ARM results.
        /// </summary>
        public const int STRIKE_OTHER_ARM = 6;
        /// <summary>
        /// Strike check passed with GUTS results.
        /// </summary>
        public const int STRIKE_GUTS = 7;
        /// <summary>
        /// Strike check passed with RIGHT LEG results.
        /// </summary>
        public const int STRIKE_RIGHT_LEG = 8;
        /// <summary>
        /// Strike check passed with LEFT LEG results.
        /// </summary>
        public const int STRIKE_LEFT_LEG = 9;
        /// <summary>
        /// Strike check passed with THROAT results.
        /// </summary>
        public const int STRIKE_THROAT = 10;
        /// <summary>
        /// Strike check passed with HEART results.
        /// </summary>
        public const int STRIKE_HEART = 11;
        /// <summary>
        /// the Strike check matrix.
        /// </summary>
        private readonly Dictionary<int, int[][]> STRIKE_MATRIX = new Dictionary<int, int[][]>()
        {
            {-6, new int[][]{ new int[] {41, STRIKE_MISS},
                new int[] { 52, STRIKE_SHIELD },
                new int[] { 63, STRIKE_TRUNK },
                new int[] { 75, STRIKE_FACE },
                new int[] { 84, STRIKE_HEAD },
                new int[] { 92, STRIKE_SWORD_ARM },
                new int[] { 97, STRIKE_OTHER_ARM },
                new int[] { 103, STRIKE_GUTS },
                new int[] { 108, STRIKE_RIGHT_LEG },
                new int[] { 112, STRIKE_LEFT_LEG },
                new int[] { 114, STRIKE_THROAT },
                new int[] { 115, STRIKE_HEART } }
            },
            { -5, new int[][]{ new int[] {40, STRIKE_MISS},
                new int[] { 49, STRIKE_SHIELD },
                new int[] { 60, STRIKE_TRUNK },
                new int[] { 70, STRIKE_FACE },
                new int[] { 79, STRIKE_HEAD },
                new int[] { 87, STRIKE_SWORD_ARM },
                new int[] { 95, STRIKE_OTHER_ARM },
                new int[] { 101, STRIKE_GUTS },
                new int[] { 106, STRIKE_RIGHT_LEG },
                new int[] { 110, STRIKE_LEFT_LEG },
                new int[] { 113, STRIKE_THROAT },
                new int[] { 115, STRIKE_HEART } }
            },
            { -4, new int[][]{ new int[] {38, STRIKE_MISS},
                new int[] { 47, STRIKE_SHIELD },
                new int[] { 57, STRIKE_TRUNK },
                new int[] { 67, STRIKE_FACE },
                new int[] { 77, STRIKE_HEAD },
                new int[] { 86, STRIKE_SWORD_ARM },
                new int[] { 94, STRIKE_OTHER_ARM },
                new int[] { 100, STRIKE_GUTS },
                new int[] { 106, STRIKE_RIGHT_LEG },
                new int[] { 110, STRIKE_LEFT_LEG },
                new int[] { 113, STRIKE_THROAT },
                new int[] { 115, STRIKE_HEART } }
            },
            { -3, new int[][]{ new int[] {37, STRIKE_MISS},
                new int[] { 45, STRIKE_SHIELD },
                new int[] { 56, STRIKE_TRUNK },
                new int[] { 66, STRIKE_FACE },
                new int[] { 76, STRIKE_HEAD },
                new int[] { 85, STRIKE_SWORD_ARM },
                new int[] { 93, STRIKE_OTHER_ARM },
                new int[] { 100, STRIKE_GUTS },
                new int[] { 106, STRIKE_RIGHT_LEG },
                new int[] { 110, STRIKE_LEFT_LEG },
                new int[] { 113, STRIKE_THROAT },
                new int[] { 115, STRIKE_HEART } }
            },
            { -2, new int[][]{ new int[] {35, STRIKE_MISS},
                new int[] { 43, STRIKE_SHIELD },
                new int[] { 55, STRIKE_TRUNK },
                new int[] { 65, STRIKE_FACE },
                new int[] { 75, STRIKE_HEAD },
                new int[] { 84, STRIKE_SWORD_ARM },
                new int[] { 93, STRIKE_OTHER_ARM },
                new int[] { 99, STRIKE_GUTS },
                new int[] { 104, STRIKE_RIGHT_LEG },
                new int[] { 108, STRIKE_LEFT_LEG },
                new int[] { 111, STRIKE_THROAT },
                new int[] { 115, STRIKE_HEART } }
            },
            { -1, new int[][]{ new int[] {34, STRIKE_MISS},
                new int[] { 42, STRIKE_SHIELD },
                new int[] { 54, STRIKE_TRUNK },
                new int[] { 64, STRIKE_FACE },
                new int[] { 74, STRIKE_HEAD },
                new int[] { 83, STRIKE_SWORD_ARM },
                new int[] { 91, STRIKE_OTHER_ARM },
                new int[] { 98, STRIKE_GUTS },
                new int[] { 103, STRIKE_RIGHT_LEG },
                new int[] { 107, STRIKE_LEFT_LEG },
                new int[] { 111, STRIKE_THROAT },
                new int[] { 115, STRIKE_HEART } }
            },
            { 0, new int[][]{ new int[] {32, STRIKE_MISS},
                new int[] { 41, STRIKE_SHIELD },
                new int[] { 53, STRIKE_TRUNK },
                new int[] { 63, STRIKE_FACE },
                new int[] { 73, STRIKE_HEAD },
                new int[] { 82, STRIKE_SWORD_ARM },
                new int[] { 90, STRIKE_OTHER_ARM },
                new int[] { 97, STRIKE_GUTS },
                new int[] { 102, STRIKE_RIGHT_LEG },
                new int[] { 107, STRIKE_LEFT_LEG },
                new int[] { 111, STRIKE_THROAT },
                new int[] { 115, STRIKE_HEART } }
            },
            { 1, new int[][]{ new int[] {31, STRIKE_MISS},
                new int[] { 39, STRIKE_SHIELD },
                new int[] { 50, STRIKE_TRUNK },
                new int[] { 60, STRIKE_FACE },
                new int[] { 70, STRIKE_HEAD },
                new int[] { 79, STRIKE_SWORD_ARM },
                new int[] { 87, STRIKE_OTHER_ARM },
                new int[] { 94, STRIKE_GUTS },
                new int[] { 99, STRIKE_RIGHT_LEG },
                new int[] { 106, STRIKE_LEFT_LEG },
                new int[] { 110, STRIKE_THROAT },
                new int[] { 115, STRIKE_HEART } }
            },
            { 2, new int[][]{ new int[] {29, STRIKE_MISS},
                new int[] { 37, STRIKE_SHIELD },
                new int[] { 48, STRIKE_TRUNK },
                new int[] { 59, STRIKE_FACE },
                new int[] { 69, STRIKE_HEAD },
                new int[] { 78, STRIKE_SWORD_ARM },
                new int[] { 86, STRIKE_OTHER_ARM },
                new int[] { 93, STRIKE_GUTS },
                new int[] { 99, STRIKE_RIGHT_LEG },
                new int[] { 106, STRIKE_LEFT_LEG },
                new int[] { 110, STRIKE_THROAT },
                new int[] { 115, STRIKE_HEART } }
            },
            { 3, new int[][]{ new int[] {28, STRIKE_MISS},
                new int[] { 36, STRIKE_SHIELD },
                new int[] { 47, STRIKE_TRUNK },
                new int[] { 58, STRIKE_FACE },
                new int[] { 68, STRIKE_HEAD },
                new int[] { 77, STRIKE_SWORD_ARM },
                new int[] { 85, STRIKE_OTHER_ARM },
                new int[] { 92, STRIKE_GUTS },
                new int[] { 98, STRIKE_RIGHT_LEG },
                new int[] { 105, STRIKE_LEFT_LEG },
                new int[] { 110, STRIKE_THROAT },
                new int[] { 115, STRIKE_HEART } }
            },
            { 4, new int[][]{ new int[] {26, STRIKE_MISS},
                new int[] { 35, STRIKE_SHIELD },
                new int[] { 46, STRIKE_TRUNK },
                new int[] { 57, STRIKE_FACE },
                new int[] { 67, STRIKE_HEAD },
                new int[] { 76, STRIKE_SWORD_ARM },
                new int[] { 84, STRIKE_OTHER_ARM },
                new int[] { 91, STRIKE_GUTS },
                new int[] { 97, STRIKE_RIGHT_LEG },
                new int[] { 103, STRIKE_LEFT_LEG },
                new int[] { 109, STRIKE_THROAT },
                new int[] { 115, STRIKE_HEART } }
            },
            { 5, new int[][]{ new int[] {25, STRIKE_MISS},
                new int[] { 33, STRIKE_SHIELD },
                new int[] { 44, STRIKE_TRUNK },
                new int[] { 55, STRIKE_FACE },
                new int[] { 65, STRIKE_HEAD },
                new int[] { 74, STRIKE_SWORD_ARM },
                new int[] { 82, STRIKE_OTHER_ARM },
                new int[] { 89, STRIKE_GUTS },
                new int[] { 95, STRIKE_RIGHT_LEG },
                new int[] { 102, STRIKE_LEFT_LEG },
                new int[] { 109, STRIKE_THROAT },
                new int[] { 115, STRIKE_HEART } }
            },
            { 6, new int[][]{ new int[] {23, STRIKE_MISS},
                new int[] { 30, STRIKE_SHIELD },
                new int[] { 41, STRIKE_TRUNK },
                new int[] { 52, STRIKE_FACE },
                new int[] { 63, STRIKE_HEAD },
                new int[] { 73, STRIKE_SWORD_ARM },
                new int[] { 81, STRIKE_OTHER_ARM },
                new int[] { 88, STRIKE_GUTS },
                new int[] { 95, STRIKE_RIGHT_LEG },
                new int[] { 102, STRIKE_LEFT_LEG },
                new int[] { 109, STRIKE_THROAT },
                new int[] { 115, STRIKE_HEART } }
            },
            { 7, new int[][]{ new int[] {22, STRIKE_MISS},
                new int[] { 28, STRIKE_SHIELD },
                new int[] { 39, STRIKE_TRUNK },
                new int[] { 50, STRIKE_FACE },
                new int[] { 61, STRIKE_HEAD },
                new int[] { 71, STRIKE_SWORD_ARM },
                new int[] { 79, STRIKE_OTHER_ARM },
                new int[] { 86, STRIKE_GUTS },
                new int[] { 93, STRIKE_RIGHT_LEG },
                new int[] { 101, STRIKE_LEFT_LEG },
                new int[] { 108, STRIKE_THROAT },
                new int[] { 115, STRIKE_HEART } }
            },
            { 8, new int[][]{ new int[] {20, STRIKE_MISS},
                new int[] { 25, STRIKE_SHIELD },
                new int[] { 37, STRIKE_TRUNK },
                new int[] { 48, STRIKE_FACE },
                new int[] { 59, STRIKE_HEAD },
                new int[] { 69, STRIKE_SWORD_ARM },
                new int[] { 77, STRIKE_OTHER_ARM },
                new int[] { 86, STRIKE_GUTS },
                new int[] { 92, STRIKE_RIGHT_LEG },
                new int[] { 100, STRIKE_LEFT_LEG },
                new int[] { 108, STRIKE_THROAT },
                new int[] { 115, STRIKE_HEART } }
            },
            { 9, new int[][]{ new int[] {19, STRIKE_MISS},
                new int[] { 23, STRIKE_SHIELD },
                new int[] { 36, STRIKE_TRUNK },
                new int[] { 47, STRIKE_FACE },
                new int[] { 58, STRIKE_HEAD },
                new int[] { 68, STRIKE_SWORD_ARM },
                new int[] { 76, STRIKE_OTHER_ARM },
                new int[] { 85, STRIKE_GUTS },
                new int[] { 91, STRIKE_RIGHT_LEG },
                new int[] { 99, STRIKE_LEFT_LEG },
                new int[] { 107, STRIKE_THROAT },
                new int[] { 115, STRIKE_HEART } }
            },
            { 10, new int[][]{ new int[] {17, STRIKE_MISS},
                new int[] { 20, STRIKE_SHIELD },
                new int[] { 35, STRIKE_TRUNK },
                new int[] { 46, STRIKE_FACE },
                new int[] { 57, STRIKE_HEAD },
                new int[] { 67, STRIKE_SWORD_ARM },
                new int[] { 75, STRIKE_OTHER_ARM },
                new int[] { 84, STRIKE_GUTS },
                new int[] { 90, STRIKE_RIGHT_LEG },
                new int[] { 99, STRIKE_LEFT_LEG },
                new int[] { 107, STRIKE_THROAT },
                new int[] { 115, STRIKE_HEART } }
            },
            { 11, new int[][]{ new int[] {16, STRIKE_MISS},
                new int[] { 18, STRIKE_SHIELD },
                new int[] { 34, STRIKE_TRUNK },
                new int[] { 45, STRIKE_FACE },
                new int[] { 56, STRIKE_HEAD },
                new int[] { 66, STRIKE_SWORD_ARM },
                new int[] { 74, STRIKE_OTHER_ARM },
                new int[] { 83, STRIKE_GUTS },
                new int[] { 89, STRIKE_RIGHT_LEG },
                new int[] { 98, STRIKE_LEFT_LEG },
                new int[] { 106, STRIKE_THROAT },
                new int[] { 115, STRIKE_HEART } }
            },
            { 12, new int[][]{ new int[] {14, STRIKE_MISS},
                new int[] { 15, STRIKE_SHIELD },
                new int[] { 33, STRIKE_TRUNK },
                new int[] { 43, STRIKE_FACE },
                new int[] { 55, STRIKE_HEAD },
                new int[] { 65, STRIKE_SWORD_ARM },
                new int[] { 73, STRIKE_OTHER_ARM },
                new int[] { 82, STRIKE_GUTS },
                new int[] { 89, STRIKE_RIGHT_LEG },
                new int[] { 98, STRIKE_LEFT_LEG },
                new int[] { 106, STRIKE_THROAT },
                new int[] { 115, STRIKE_HEART } }
            },
            { 13, new int[][]{ new int[] {12, STRIKE_MISS},
                new int[] { 13, STRIKE_SHIELD },
                new int[] { 32, STRIKE_TRUNK },
                new int[] { 41, STRIKE_FACE },
                new int[] { 53, STRIKE_HEAD },
                new int[] { 64, STRIKE_SWORD_ARM },
                new int[] { 72, STRIKE_OTHER_ARM },
                new int[] { 81, STRIKE_GUTS },
                new int[] { 88, STRIKE_RIGHT_LEG },
                new int[] { 97, STRIKE_LEFT_LEG },
                new int[] { 105, STRIKE_THROAT },
                new int[] { 115, STRIKE_HEART } }
            },
            { 14, new int[][]{ new int[] {10, STRIKE_MISS},
                new int[] { 11, STRIKE_SHIELD },
                new int[] { 31, STRIKE_TRUNK },
                new int[] { 39, STRIKE_FACE },
                new int[] { 52, STRIKE_HEAD },
                new int[] { 63, STRIKE_SWORD_ARM },
                new int[] { 71, STRIKE_OTHER_ARM },
                new int[] { 80, STRIKE_GUTS },
                new int[] { 87, STRIKE_RIGHT_LEG },
                new int[] { 96, STRIKE_LEFT_LEG },
                new int[] { 105, STRIKE_THROAT },
                new int[] { 115, STRIKE_HEART } }
            }
        };
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
                        FWCharacter pc = (FWCharacter)srcIo.PcData;
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
                            damages = GetDamages((FWItemData)io.ItemData, result);
                            // add surplus physique
                            damages += pc.SurplusPhysique;
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
        public const int INITIAL_STRIKE = 1;
        /// <summary>
        /// Calculates the deflection value for a piece of armour based on the targeted area.
        /// </summary>
        /// <param name="targetIo">the IO being targeted</param>
        /// <param name="targetArea">the area being targeted</param>
        /// <returns>float</returns>
        public float CalculateArmorDeflection(BaseInteractiveObject targetIo, int targetArea)
        {
            float absorb = 0;
            if (targetIo.HasIOFlag(IoGlobals.IO_01_PC))
            {
                BaseInteractiveObject armorIo = null;
                switch (targetArea)
                {
                    case STRIKE_FACE:
                    case STRIKE_HEAD:
                        if (targetIo.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_HELMET) >= 0)
                        {
                            BaseInteractiveObject io = Interactive.Instance.GetIO(targetIo.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_HELMET));
                            armorIo = io;
                            if (targetArea == STRIKE_FACE
                                && !((FWItemData)io.ItemData).IsFaced)
                            {
                                armorIo = null;
                            }
                        }
                        break;
                    case STRIKE_GUTS:
                    case STRIKE_HEART:
                    case STRIKE_TRUNK:
                        if (targetIo.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_TORSO) >= 0)
                        {
                            BaseInteractiveObject io = Interactive.Instance.GetIO(targetIo.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_TORSO));
                            armorIo = io;
                        }
                        break;
                    case STRIKE_LEFT_LEG:
                    case STRIKE_RIGHT_LEG:
                        if (targetIo.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_TORSO) >= 0) // WEARING ARMOUR
                        {
                            BaseInteractiveObject io = Interactive.Instance.GetIO(targetIo.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_TORSO));
                            armorIo = io;
                            if (!((FWItemData)io.ItemData).IsSkirted) // ARMOUR IS NOT SKIRTED
                            {
                                armorIo = null;
                                if (targetIo.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_LEGGINGS) >= 0) // WEARING LEGGINGS
                                {
                                    io = Interactive.Instance.GetIO(targetIo.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_LEGGINGS));
                                    armorIo = io;
                                }
                            }
                        }
                        else if (targetIo.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_LEGGINGS) >= 0) // NO ARMOUR, WEARING LEGGINGS
                        {
                            BaseInteractiveObject io = Interactive.Instance.GetIO(targetIo.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_LEGGINGS));
                            armorIo = io;
                        }
                        break;
                    case STRIKE_OTHER_ARM:
                    case STRIKE_SWORD_ARM:
                        if (targetIo.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_TORSO) >= 0) // WEARING ARMOUR
                        {
                            BaseInteractiveObject io = Interactive.Instance.GetIO(targetIo.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_TORSO));
                            armorIo = io;
                            if (!((FWItemData)io.ItemData).IsSleeved) // ARMOUR IS NOT SLEEVED
                            {
                                armorIo = null;
                            }
                        }
                        break;
                    case STRIKE_SHIELD:
                        if (targetIo.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_SHIELD) >= 0)
                        {
                            BaseInteractiveObject io = Interactive.Instance.GetIO(targetIo.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_SHIELD));
                            armorIo = io;
                        }
                        else if (targetIo.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_TORSO) >= 0)
                        {
                            BaseInteractiveObject io = Interactive.Instance.GetIO(targetIo.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_TORSO));
                            armorIo = io;
                        }
                        break;
                    case STRIKE_THROAT:
                        if (targetIo.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_TORSO) >= 0) // WEARING ARMOUR
                        {
                            BaseInteractiveObject io = Interactive.Instance.GetIO(targetIo.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_TORSO));
                            armorIo = io;
                            if (!((FWItemData)io.ItemData).IsCollared) // ARMOUR IS NOT COLLARED
                            {
                                armorIo = null;
                            }
                        }
                        break;
                }
                if (armorIo != null)
                {
                    absorb = ((FWItemData)armorIo.ItemData).ArmourValue;
                }
            }
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
        private int GetDamages(FWItemData item, int strikeResult)
        {
            int damage = item.Dice.Roll();
            damage += item.DmgModifier;
            switch (strikeResult)
            {
                case STRIKE_HEART:
                case STRIKE_THROAT:
                    damage *= 2;
                    break;
                default:
                    break;
            }
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
            if (srcIo.HasIOFlag(IoGlobals.IO_01_PC)
                && wpnIo.HasIOFlag(IoGlobals.IO_02_ITEM))
            {
                // TODO add combat level
                FWCharacter pc = (FWCharacter)srcIo.PcData;
                mod += pc.SurplusAgility;
                // -1 for INT 4-8
                if (pc.CheckAttributeRange("INT", 4, 8))
                {
                    mod--;
                }
                // -1 for BRV 4-8
                if (!wpnIo.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_BOW)
                    && pc.CheckAttributeRange("BRV", 4, 8))
                {
                    mod--;
                }
                // -1 for MAX END 4-8
                if (!wpnIo.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_BOW)
                    && pc.CheckAttributeRange("MEND", 4, 8))
                {
                    mod--;
                }
                // -1 for being berserk
                if (srcIo.Script.GetLocalIntVariableValue("berserk_check") == HeroBase.BERSERK_BERSERK)
                {
                    mod--;
                }
                // TODO -1 for being outnumbered per outnumbered
                // -2 for INT 0-3
                if (pc.CheckAttributeRange("INT", 0, 3))
                {
                    mod -= 2;
                }
                // -2 for BRV 0-3
                if (!wpnIo.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_BOW)
                    && pc.CheckAttributeRange("BRV", 0, 3))
                {
                    mod -= 2;
                }
                // -2 for MEND 0-3
                if (!wpnIo.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_BOW)
                    && pc.CheckAttributeRange("MEND", 0, 3))
                {
                    mod -= 2;
                }
                // TODO -2 for being damaged in last flurry
                // TODO -2 for using non-favored weapon
                // TODO -2 for blow carrying over from a parry where the opponent's weapon broke *
                // TODO -2 for last blow was partially dodged or parried
                // TODO -2 for character exhausted
                // TODO -3 for last blow was substantially parried
                // TODO -4 for last blow was substantially dodged
                // +1 for INT 14+
                if (!wpnIo.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_BOW)
                    && pc.CheckAttributeRange("INT", 14, 999))
                {
                    mod++;
                }
                // +1 for MEND 14+
                if (!wpnIo.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_BOW)
                    && pc.CheckAttributeRange("MEND", 14, 999))
                {
                    mod++;
                }
                // TODO +1 for opponent being damaged in last flurry
                // TODO +1 for opponent exhausted
                // TODO +1 for win initiative
                // TODO +1 for being opponent outnumbered per outnumbering (+3 max)
                // TODO +3 free hack on unaware, fleeing, or stunned opponent

                pc = null;
            }
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
                if (result > STRIKE_MISS)
                {
                    dmgs = ComputeDamages(srcIo, wpnIo, targIo, result);
                    // damage the target
                    if (targIo.HasIOFlag(IoGlobals.IO_01_PC))
                    {
                        targIo.PcData.DamagePlayer(dmgs, 0, srcIo.RefId);
                    }
                    else if (targIo.HasIOFlag(IoGlobals.IO_03_NPC))
                    {
                        targIo.NpcData.DamageNPC(dmgs, srcIo.RefId, false);
                    }
                    // TODO handle special effects, such as paralyze or drain life
                    // TODO - check if weapon durability is affected
                    switch (result)
                    {
                        case STRIKE_HEAD:
                            if (dmgs >= 4)
                            {
                                if (!targIo.Script.HasLocalVariable("stun_round_start"))
                                {
                                    // target is stunned for D4 flurries
                                    targIo.Script.SetLocalVariable("stunned_duration", Diceroller.Instance.RolldX(4));
                                    targIo.Script.SetLocalVariable("stun_round_start", FlurryNumber);
                                }
                            }
                            break;
                        case STRIKE_FACE:
                            if (dmgs >=3)
                            {
                                if (!targIo.Script.HasLocalVariable("blind_round_start"))
                                {
                                    // target is stunned for D4 flurries
                                    targIo.Script.SetLocalVariable("blind_duration", Diceroller.Instance.RolldX(4));
                                    targIo.Script.SetLocalVariable("blind_round_start", FlurryNumber);
                                }
                            }
                            break;
                        case STRIKE_GUTS:
                            if (dmgs >= 4)
                            {
                                targIo.Script.SetLocalVariable("disengaging", 1);
                            }
                            break;
                    }
                }
                // save the damage the attack caused
                if ((flags & INITIAL_STRIKE) == INITIAL_STRIKE)
                {
                    targIo.Script.SetLocalVariable("initial_strike_dmg", dmgs);
                    targIo.Script.SetLocalVariable("initial_strike_result", result);
                }
                // store message about attack
                Messages.Instance.Add(BuildMessage(srcIo, targIo, result, dmgs));
            }
            return false;
        }
        public int FlurryNumber { get; set; }
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
            switch (result)
            {
                case STRIKE_MISS:
                    sb.Append(" slashes wildly at ");
                    sb.Append(targName);
                    sb.Append(" but misses.\n");
                    break;
                case STRIKE_FACE:
                    sb.Append(" strikes ");
                    sb.Append(targName);
                    sb.Append(" in the face ");
                    if (damages == 0)
                    {
                        sb.Append(" but cannot penetrate ");
                        if (target.HasIOFlag(IoGlobals.IO_01_PC))
                        {
                            sb.Append(Gender.GENDER_POSSESSIVE[target.PcData.Gender]);
                        }
                        else
                        {
                            sb.Append(Gender.GENDER_POSSESSIVE[target.NpcData.Gender]);
                        }
                        sb.Append(" helmet.");
                    }
                    else
                    {
                        sb.Append(" causing ");
                        sb.Append((int)damages);
                        sb.Append(" damage!\n");
                    }
                    break;
                case STRIKE_HEAD:
                    sb.Append(" slams ");
                    sb.Append(targName);
                    sb.Append(" in the head ");
                    if (damages == 0)
                    {
                        sb.Append("but they are saved by ");
                        if (target.HasIOFlag(IoGlobals.IO_01_PC))
                        {
                            sb.Append(Gender.GENDER_POSSESSIVE[target.PcData.Gender]);
                        }
                        else
                        {
                            sb.Append(Gender.GENDER_POSSESSIVE[target.NpcData.Gender]);
                        }
                        sb.Append(" helmet!");
                    }
                    else
                    {
                        sb.Append("for ");
                        sb.Append((int)damages);
                        sb.Append(" damage!\n");
                    }
                    break;
                case STRIKE_GUTS:
                    sb.Append(" slashes ");
                    sb.Append(targName);
                    sb.Append(" across the abdomen ");
                    if (damages == 0)
                    {
                        sb.Append(" but failing to penetrate ");
                        if (target.HasIOFlag(IoGlobals.IO_01_PC))
                        {
                            sb.Append(Gender.GENDER_POSSESSIVE[target.PcData.Gender]);
                        }
                        else
                        {
                            sb.Append(Gender.GENDER_POSSESSIVE[target.NpcData.Gender]);
                        }
                        sb.Append(" armour.");
                    }
                    else
                    {
                        sb.Append(" - slicing ");
                        if (target.HasIOFlag(IoGlobals.IO_01_PC))
                        {
                            sb.Append(Gender.GENDER_OBJECTIVE[target.PcData.Gender]);
                        } else
                        {
                            sb.Append(Gender.GENDER_OBJECTIVE[target.NpcData.Gender]);
                        }
                        sb.Append(" for ");
                        sb.Append((int)damages);
                        sb.Append(" damage!\n");
                    }
                    break;
                case STRIKE_HEART:
                    sb.Append(" stabs ");
                    sb.Append(targName);
                    sb.Append(" in the heart");
                    if (damages == 0)
                    {
                        sb.Append(", denting ");
                        if (target.HasIOFlag(IoGlobals.IO_01_PC))
                        {
                            sb.Append(Gender.GENDER_POSSESSIVE[target.PcData.Gender]);
                        }
                        else
                        {
                            sb.Append(Gender.GENDER_POSSESSIVE[target.NpcData.Gender]);
                        }
                        sb.Append(" armour but causing nor further damage.");
                    }
                    else
                    {
                        sb.Append("! ");
                        sb.Append((int)damages);
                        sb.Append(" damage!\n");
                    }
                    break;
                case STRIKE_TRUNK:
                    sb.Append(" thrusts at ");
                    sb.Append(targName);
                    if (damages == 0)
                    {
                        sb.Append(" who is saved by ");
                        if (target.HasIOFlag(IoGlobals.IO_01_PC))
                        {
                            sb.Append(Gender.GENDER_POSSESSIVE[target.PcData.Gender]);
                        }
                        else
                        {
                            sb.Append(Gender.GENDER_POSSESSIVE[target.NpcData.Gender]);
                        }
                        sb.Append(" armour.");
                    }
                    else
                    {
                        sb.Append(" catching ");
                        if (target.HasIOFlag(IoGlobals.IO_01_PC))
                        {
                            sb.Append(Gender.GENDER_OBJECTIVE[target.PcData.Gender]);
                        }
                        else
                        {
                            sb.Append(Gender.GENDER_OBJECTIVE[target.NpcData.Gender]);
                        }
                        sb.Append(" in the ribs for ");
                        sb.Append((int)damages);
                        sb.Append(" damage!\n");
                    }
                    break;
                case STRIKE_LEFT_LEG:
                case STRIKE_RIGHT_LEG:
                    sb.Append(" swings at ");
                    sb.Append(targName);
                    sb.Append("'s legs");
                    if (damages == 0)
                    {
                        sb.Append(" but only lands a glancing blow.");
                    }
                    else
                    {
                        sb.Append(" opening a gash - ");
                        sb.Append((int)damages);
                        sb.Append(" damage!\n");
                    }
                    break;
                case STRIKE_OTHER_ARM:
                    sb.Append(" swings at ");
                    sb.Append(targName);
                    sb.Append("'s free arm");
                    if (damages == 0)
                    {
                        sb.Append(" but only lands a glancing blow.");
                    }
                    else
                    {
                        sb.Append(" opening a gash - ");
                        sb.Append((int)damages);
                        sb.Append(" damage!\n");
                    }
                    break;
                case STRIKE_SWORD_ARM:
                    sb.Append(" swings at ");
                    sb.Append(targName);
                    sb.Append("'s sword arm");
                    if (damages == 0)
                    {
                        sb.Append(" but only lands a glancing blow.");
                    }
                    else
                    {
                        sb.Append(" opening a gash - ");
                        sb.Append((int)damages);
                        sb.Append(" damage!\n");
                    }
                    break;
                case STRIKE_SHIELD:
                    sb.Append(" swings at  ");
                    sb.Append(targName);
                    sb.Append("'s shielded side");
                    if (damages == 0)
                    {
                        sb.Append(" but only lands a glancing blow.");
                    }
                    else
                    {
                        sb.Append(" breaking through ");
                        if (target.HasIOFlag(IoGlobals.IO_01_PC))
                        {
                            sb.Append(Gender.GENDER_POSSESSIVE[target.PcData.Gender]);
                        }
                        else
                        {
                            sb.Append(Gender.GENDER_POSSESSIVE[target.NpcData.Gender]);
                        }
                        sb.Append(" defenses and scoring a wound for ");
                        sb.Append((int)damages);
                        sb.Append(" damage!\n");
                    }
                    break;
                case STRIKE_THROAT:
                    sb.Append(" chops at ");
                    sb.Append(targName);
                    sb.Append("'s throat");
                    if (damages == 0)
                    {
                        sb.Append(" but only lands a glancing blow.");
                    }
                    else
                    {
                        sb.Append(" breaking through ");
                        if (target.HasIOFlag(IoGlobals.IO_01_PC))
                        {
                            sb.Append(Gender.GENDER_POSSESSIVE[target.PcData.Gender]);
                        }
                        else
                        {
                            sb.Append(Gender.GENDER_POSSESSIVE[target.NpcData.Gender]);
                        }
                        sb.Append(" defenses and scoring a wound for ");
                        sb.Append((int)damages);
                        sb.Append(" damage!\n");
                    }
                    break;
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
            int[][] matrix = STRIKE_MATRIX[key];
            int result = 0;
            for (int i = 0, li = matrix.Length; i < li; i++)
            {
                if (roll <= matrix[i][0])
                {
                    result = matrix[i][1];
                    break;
                }
            }
            return result;
        }
    }
}
