using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabLord.Constants
{
    public sealed class LabLordSaveThrow
    {
        /// <summary>
        /// Save vs Breath Weapon
        /// </summary>
        public const int SAVE_V_BREATH = 0;
        /// <summary>
        /// Save vs Poison
        /// </summary>
        public const int SAVE_V_POISON = 1;
        /// <summary>
        /// Save vs Death
        /// </summary>
        public const int SAVE_V_DEATH = 2;
        /// <summary>
        /// Save vs Petrify
        /// </summary>
        public const int SAVE_V_PETRIFY = 3;
        /// <summary>
        /// Save vs PARALYZE
        /// </summary>
        public const int SAVE_V_PARALYZE = 4;
        /// <summary>
        /// Save vs WANDS
        /// </summary>
        public const int SAVE_V_WANDS = 5;
        /// <summary>
        /// Save vs SPELLS
        /// </summary>
        public const int SAVE_V_SPELLS = 6;
        public static int[] EQUIP_ELEMENT_MAP = {
            LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH,
            LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON,
            LabLordGlobals.EQUIP_ELEMENT_SAVE_V_DEATH,
            LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PETRIFY,
            LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PARALYZE,
            LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,
            LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS
        };
        private static Dictionary<int, Dictionary<int, int>> THROWS_CLERIC_DRUID_MONK = new Dictionary<int, Dictionary<int, int>>
        {
            {
                SAVE_V_BREATH,
                new Dictionary<int, int> {
                    { 1, 16 }, { 2, 16 }, { 3, 16 }, { 4, 16 },
                    { 5, 14 }, { 6, 14 }, { 7, 14 }, { 8, 14 },
                    { 9, 12 }, { 10, 12 }, { 11, 12 }, { 12, 12 },
                    { 13, 8 }, { 14, 8 }, { 15, 8 }, { 16, 8 },
                    { 17, 6 }
                }
            },
            {
                SAVE_V_POISON,
                new Dictionary<int, int> {
                    { 1, 11 }, { 2, 11 }, { 3, 11 }, { 4, 11 },
                    { 5, 9 }, { 6, 9 }, { 7, 9 }, { 8, 9 },
                    { 9, 7 }, { 10, 7 }, { 11, 7 }, { 12, 7 },
                    { 13, 3 }, { 14, 3 }, { 15, 3 }, { 16, 3 },
                    { 17, 2 }
                }
            },
            {
                SAVE_V_DEATH,
                new Dictionary<int, int> {
                    { 1, 11 }, { 2, 11 }, { 3, 11 }, { 4, 11 },
                    { 5, 9 }, { 6, 9 }, { 7, 9 }, { 8, 9 },
                    { 9, 7 }, { 10, 7 }, { 11, 7 }, { 12, 7 },
                    { 13, 3 }, { 14, 3 }, { 15, 3 }, { 16, 3 },
                    { 17, 2 }
                }
            },
            {
                SAVE_V_PETRIFY,
                new Dictionary<int, int> {
                    { 1, 14 }, { 2, 14 }, { 3, 14 }, { 4, 14 },
                    { 5, 12 }, { 6, 12 }, { 7, 12 }, { 8, 12 },
                    { 9, 10 }, { 10, 10 }, { 11, 10 }, { 12, 10 },
                    { 13, 8 }, { 14, 8 }, { 15, 8 }, { 16, 8 },
                    { 17, 6 }
                }
            },
            {
                SAVE_V_PARALYZE,
                new Dictionary<int, int> {
                    { 1, 14 }, { 2, 14 }, { 3, 14 }, { 4, 14 },
                    { 5, 12 }, { 6, 12 }, { 7, 12 }, { 8, 12 },
                    { 9, 10 }, { 10, 10 }, { 11, 10 }, { 12, 10 },
                    { 13, 8 }, { 14, 8 }, { 15, 8 }, { 16, 8 },
                    { 17, 6 }
                }
            },
            {
                SAVE_V_WANDS,
                new Dictionary<int, int> {
                    { 1, 12 }, { 2, 12 }, { 3, 12 }, { 4, 12 },
                    { 5, 10 }, { 6, 10 }, { 7, 10 }, { 8, 10 },
                    { 9, 8 }, { 10, 8 }, { 11, 8 }, { 12, 8 },
                    { 13, 4 }, { 14, 4 }, { 15, 4 }, { 16, 4 },
                    { 17, 4 }
                }
            },
            {
                SAVE_V_SPELLS,
                new Dictionary<int, int> {
                    { 1, 15 }, { 2, 15 }, { 3, 15 }, { 4, 15 },
                    { 5, 12 }, { 6, 12 }, { 7, 12 }, { 8, 12 },
                    { 9, 9 }, { 10, 9 }, { 11, 9 }, { 12, 9 },
                    { 13, 6 }, { 14, 6 }, { 15, 6 }, { 16, 6 },
                    { 17, 5 }
                }
            }
        };
        private static Dictionary<int, Dictionary<int, int>> THROWS_FIGHTER_PALADIN_RANGER = new Dictionary<int, Dictionary<int, int>>
        {
            {
                SAVE_V_BREATH,
                new Dictionary<int, int> {
                    { 0, 17 },
                    { 1, 15 }, { 2, 15 }, { 3, 15 },
                    { 4, 13 }, { 5, 13 }, { 6, 13 },
                    { 7, 9 }, { 8, 9 }, { 9, 9 },
                    { 10, 7 }, { 11, 7 }, { 12, 7 },
                    { 13, 5 }, { 14, 5 }, { 15, 5 },
                    { 16, 4 }, { 17, 4 }, { 18, 4 },
                    { 19, 4 }
                }
            },
            {
                SAVE_V_POISON,
                new Dictionary<int, int> {
                    { 0, 14 },
                    { 1, 12 }, { 2, 12 }, { 3, 12 },
                    { 4, 10 }, { 5, 10 }, { 6, 10 },
                    { 7, 8 }, { 8, 8 }, { 9, 8 },
                    { 10, 6 }, { 11, 6 }, { 12, 6 },
                    { 13, 4 }, { 14, 4 }, { 15, 4 },
                    { 16, 4 }, { 17, 4 }, { 18, 4 },
                    { 19, 3 }
                }
            },
            {
                SAVE_V_DEATH,
                new Dictionary<int, int> {
                    { 0, 14 },
                    { 1, 12 }, { 2, 12 }, { 3, 12 },
                    { 4, 10 }, { 5, 10 }, { 6, 10 },
                    { 7, 8 }, { 8, 8 }, { 9, 8 },
                    { 10, 6 }, { 11, 6 }, { 12, 6 },
                    { 13, 4 }, { 14, 4 }, { 15, 4 },
                    { 16, 4 }, { 17, 4 }, { 18, 4 },
                    { 19, 3 }
                }
            },
            {
                SAVE_V_PETRIFY,
                new Dictionary<int, int> {
                    { 0, 16 },
                    { 1, 14 }, { 2, 14 }, { 3, 14 },
                    { 4, 12 }, { 5, 12 }, { 6, 12 },
                    { 7, 10 }, { 8, 10 }, { 9, 10 },
                    { 10, 8 }, { 11, 8 }, { 12, 8 },
                    { 13, 6 }, { 14, 6 }, { 15, 6 },
                    { 16, 5 }, { 17, 5 }, { 18, 5 },
                    { 19, 4 }
                }
            },
            {
                SAVE_V_PARALYZE,
                new Dictionary<int, int> {
                    { 0, 16 },
                    { 1, 14 }, { 2, 14 }, { 3, 14 },
                    { 4, 12 }, { 5, 12 }, { 6, 12 },
                    { 7, 10 }, { 8, 10 }, { 9, 10 },
                    { 10, 8 }, { 11, 8 }, { 12, 8 },
                    { 13, 6 }, { 14, 6 }, { 15, 6 },
                    { 16, 5 }, { 17, 5 }, { 18, 5 },
                    { 19, 4 }
                }
            },
            {
                SAVE_V_WANDS,
                new Dictionary<int, int> {
                    { 0, 15 },
                    { 1, 13 }, { 2, 13 }, { 3, 13 },
                    { 4, 11 }, { 5, 11 }, { 6, 11 },
                    { 7, 9 }, { 8, 9 }, { 9, 9 },
                    { 10, 7 }, { 11, 7 }, { 12, 7 },
                    { 13, 5 }, { 14, 5 }, { 15, 5 },
                    { 16, 4 }, { 17, 4 }, { 18, 4 },
                    { 19, 3 }
                }
            },
            {
                SAVE_V_SPELLS,
                new Dictionary<int, int> {
                    { 0, 18 },
                    { 1, 16 }, { 2, 16 }, { 3, 16 },
                    { 4, 14 }, { 5, 14 }, { 6, 14 },
                    { 7, 12 }, { 8, 12 }, { 9, 12 },
                    { 10, 10 }, { 11, 10 }, { 12, 10 },
                    { 13, 8 }, { 14, 8 }, { 15, 8 },
                    { 16, 7 }, { 17, 7 }, { 18, 7 },
                    { 19, 6 }
                }
            }
        };
        private static Dictionary<int, Dictionary<int, int>> THROWS_ILLUSIONIST_MAGE = new Dictionary<int, Dictionary<int, int>>
        {
            {
                SAVE_V_BREATH,
                new Dictionary<int, int> {
                    { 1, 16 }, { 2, 16 }, { 3, 16 }, { 4, 16 }, { 5, 16 },
                    { 6, 14 }, { 7, 14 }, { 8, 14 }, { 9, 14 }, { 10, 14 },
                    { 11, 12 }, { 12, 12 }, { 13, 12 }, { 14, 12 }, { 15, 12 },
                    { 16, 8 }, { 17, 8 }, { 18, 8 },
                    { 19, 7 }
                }
            },
            {
                SAVE_V_POISON,
                new Dictionary<int, int> {
                    { 1, 13 }, { 2, 13 }, { 3, 13 }, { 4, 13 }, { 5, 13 },
                    { 6, 11 }, { 7, 11 }, { 8, 11 }, { 9, 11 }, { 10, 11 },
                    { 11, 9 }, { 12, 9 }, { 13, 9 }, { 14, 9 }, { 15, 9 },
                    { 16, 7 }, { 17, 7 }, { 18, 7 },
                    { 19, 6 }
                }
            },
            {
                SAVE_V_DEATH,
                new Dictionary<int, int> {
                    { 1, 13 }, { 2, 13 }, { 3, 13 }, { 4, 13 }, { 5, 13 },
                    { 6, 11 }, { 7, 11 }, { 8, 11 }, { 9, 11 }, { 10, 11 },
                    { 11, 9 }, { 12, 9 }, { 13, 9 }, { 14, 9 }, { 15, 9 },
                    { 16, 7 }, { 17, 7 }, { 18, 7 },
                    { 19, 6 }
                }
            },
            {
                SAVE_V_PETRIFY,
                new Dictionary<int, int> {
                    { 1, 13 }, { 2, 13 }, { 3, 13 }, { 4, 13 }, { 5, 13 },
                    { 6, 11 }, { 7, 11 }, { 8, 11 }, { 9, 11 }, { 10, 11 },
                    { 11, 9 }, { 12, 9 }, { 13, 9 }, { 14, 9 }, { 15, 9 },
                    { 16, 6 }, { 17, 6 }, { 18, 6 },
                    { 19, 5 }
                }
            },
            {
                SAVE_V_PARALYZE,
                new Dictionary<int, int> {
                    { 1, 13 }, { 2, 13 }, { 3, 13 }, { 4, 13 }, { 5, 13 },
                    { 6, 11 }, { 7, 11 }, { 8, 11 }, { 9, 11 }, { 10, 11 },
                    { 11, 9 }, { 12, 9 }, { 13, 9 }, { 14, 9 }, { 15, 9 },
                    { 16, 6 }, { 17, 6 }, { 18, 6 },
                    { 19, 5 }
                }
            },
            {
                SAVE_V_WANDS,
                new Dictionary<int, int> {
                    { 1, 13 }, { 2, 13 }, { 3, 13 }, { 4, 13 }, { 5, 13 },
                    { 6, 11 }, { 7, 11 }, { 8, 11 }, { 9, 11 }, { 10, 11 },
                    { 11, 9 }, { 12, 9 }, { 13, 9 }, { 14, 9 }, { 15, 9 },
                    { 16, 5 }, { 17, 5 }, { 18, 5 },
                    { 19, 4 }
                }
            },
            {
                SAVE_V_SPELLS,
                new Dictionary<int, int> {
                    { 1, 14 }, { 2, 14 }, { 3, 14 }, { 4, 14 }, { 5, 14 },
                    { 6, 12 }, { 7, 12 }, { 8, 12 }, { 9, 12 }, { 10, 12 },
                    { 11, 8 }, { 12, 8 }, { 13, 8 }, { 14, 8 }, { 15, 8 },
                    { 16, 6 }, { 17, 6 }, { 18, 6 },
                    { 19, 4 }
                }
            }
        };
        private static Dictionary<int, Dictionary<int, int>> THROWS_ASSASSIN_THIEF = new Dictionary<int, Dictionary<int, int>>
        {
            {
                SAVE_V_BREATH,
                new Dictionary<int, int> {
                    { 1, 16 }, { 2, 16 }, { 3, 16 }, { 4, 16 },
                    { 5, 14 }, { 6, 14 }, { 7, 14 }, { 8, 14 },
                    { 9, 12 }, { 10, 12 }, { 11, 12 }, { 12, 12 },
                    { 13, 10 }, { 14, 10 }, { 15, 10 }, { 16, 10 },
                    { 17, 8 }
                }
            },
            {
                SAVE_V_POISON,
                new Dictionary<int, int> {
                    { 1, 14 }, { 2, 14 }, { 3, 14 }, { 4, 14 },
                    { 5, 12 }, { 6, 12 }, { 7, 12 }, { 8, 12 },
                    { 9, 10 }, { 10, 10 }, { 11, 10 }, { 12, 10 },
                    { 13, 8 }, { 14, 8 }, { 15, 8 }, { 16, 8 },
                    { 17, 6 }
                }
            },
            {
                SAVE_V_DEATH,
                new Dictionary<int, int> {
                    { 1, 14 }, { 2, 14 }, { 3, 14 }, { 4, 14 },
                    { 5, 12 }, { 6, 12 }, { 7, 12 }, { 8, 12 },
                    { 9, 10 }, { 10, 10 }, { 11, 10 }, { 12, 10 },
                    { 13, 8 }, { 14, 8 }, { 15, 8 }, { 16, 8 },
                    { 17, 6 }
                }
            },
            {
                SAVE_V_PETRIFY,
                new Dictionary<int, int> {
                    { 1, 13 }, { 2, 13 }, { 3, 13 }, { 4, 13 },
                    { 5, 11 }, { 6, 11 }, { 7, 11 }, { 8, 11 },
                    { 9, 9 }, { 10, 9 }, { 11, 9 }, { 12, 9 },
                    { 13, 7 }, { 14, 7 }, { 15, 7 }, { 16, 7 },
                    { 17, 5 }
                }
            },
            {
                SAVE_V_PARALYZE,
                new Dictionary<int, int> {
                    { 1, 13 }, { 2, 13 }, { 3, 13 }, { 4, 13 },
                    { 5, 11 }, { 6, 11 }, { 7, 11 }, { 8, 11 },
                    { 9, 9 }, { 10, 9 }, { 11, 9 }, { 12, 9 },
                    { 13, 7 }, { 14, 7 }, { 15, 7 }, { 16, 7 },
                    { 17, 5 }
                }
            },
            {
                SAVE_V_WANDS,
                new Dictionary<int, int> {
                    { 1, 15 }, { 2, 15 }, { 3, 15 }, { 4, 15 },
                    { 5, 13 }, { 6, 13 }, { 7, 13 }, { 8, 13 },
                    { 9, 11 }, { 10, 11 }, { 11, 11 }, { 12, 11 },
                    { 13, 9 }, { 14, 9 }, { 15, 9 }, { 16, 9 },
                    { 17, 7 }
                }
            },
            {
                SAVE_V_SPELLS,
                new Dictionary<int, int> {
                    { 1, 14 }, { 2, 14 }, { 3, 14 }, { 4, 14 },
                    { 5, 12 }, { 6, 12 }, { 7, 12 }, { 8, 12 },
                    { 9, 10 }, { 10, 10 }, { 11, 10 }, { 12, 10 },
                    { 13, 8 }, { 14, 8 }, { 15, 8 }, { 16, 8 },
                    { 17, 6 }
                }
            }
        };
        private static Dictionary<int, Dictionary<int, Dictionary<int, int>>> SAVE_THROWS = new Dictionary<int, Dictionary<int, Dictionary<int, int>>> {
            { LabLordClass.CLASS_CLERIC, THROWS_CLERIC_DRUID_MONK },
            { LabLordClass.CLASS_DRUID, THROWS_CLERIC_DRUID_MONK },
            { LabLordClass.CLASS_MONK, THROWS_CLERIC_DRUID_MONK },
            { LabLordClass.CLASS_FIGHTER, THROWS_FIGHTER_PALADIN_RANGER },
            { LabLordClass.CLASS_PALADIN, THROWS_FIGHTER_PALADIN_RANGER },
            { LabLordClass.CLASS_RANGER, THROWS_FIGHTER_PALADIN_RANGER },
            { LabLordClass.CLASS_ILLUSIONIST, THROWS_ILLUSIONIST_MAGE },
            { LabLordClass.CLASS_MAGIC_USER, THROWS_ILLUSIONIST_MAGE },
            { LabLordClass.CLASS_ASSASSIN, THROWS_ASSASSIN_THIEF },
            { LabLordClass.CLASS_THIEF, THROWS_ASSASSIN_THIEF }
        };
        /// <summary>
        /// Saving Throw titles.
        /// </summary>
        public static string[] SAVE_TITLES = {
            "Save vs Breath Weapon",
            "Save vs Poison",
            "Save vs Death",
            "Save vs Petrify",
            "Save vs Paralyze",
            "Save vs Wands",
            "Save vs Spells"
        };
        /// <summary>
        /// Saving Throw titles.
        /// </summary>
        public static string[] SAVE_SHORT_TITLES = {
            "vs Breath Weapon",
            "vs Poison",
            "vs Death",
            "vs Petrify",
            "vs Paralyze",
            "vs Wands",
            "vs Spells"
        };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="savingThrowType"></param>
        /// <param name="clazz"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static int GetSavingThrow(int savingThrowType, int clazz, int level)
        {
            int levelMax = level;
            switch (clazz)
            {
                case LabLordClass.CLASS_CLERIC:
                case LabLordClass.CLASS_DRUID:
                case LabLordClass.CLASS_MONK:
                case LabLordClass.CLASS_ASSASSIN:
                case LabLordClass.CLASS_THIEF:
                    if (level > 17)
                    {
                        levelMax = 17;
                    }
                    break;
                case LabLordClass.CLASS_FIGHTER:
                case LabLordClass.CLASS_PALADIN:
                case LabLordClass.CLASS_RANGER:
                case LabLordClass.CLASS_ILLUSIONIST:
                case LabLordClass.CLASS_MAGIC_USER:
                    if (level > 19)
                    {
                        levelMax = 19;
                    }
                    break;
            }
            return SAVE_THROWS[clazz][savingThrowType][levelMax];
        }
    }
}
