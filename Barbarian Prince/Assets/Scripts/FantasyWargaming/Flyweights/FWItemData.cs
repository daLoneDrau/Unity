using Assets.Scripts.FantasyWargaming.Scriptables.Mobs;
using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.FantasyWargaming.Flyweights
{
    public class FWItemData : IOItemData
    {
        /// <summary>
        /// the minimum physique needed to wield the item.
        /// </summary>
        public float MinMeleePhysique { get; set; }
        /// <summary>
        /// the minimum Agility needed to wield the item.
        /// </summary>
        public float MinMeleeAgility { get; set; }
        /// <summary>
        /// the item's striking range.
        /// </summary>
        public float Range { get; set; }
        public Dice Dice { get; set; }
        public int DmgModifier { get; set; }
        public bool Parry { get; set; }
        public int ParryModifier { get; set; }
        public char BreakCode { get; set; }
        protected override float ApplyCriticalModifier()
        {
            throw new NotImplementedException();
        }

        protected override float CalculateArmorDeflection()
        {
            throw new NotImplementedException();
        }
        protected override float GetBackstabModifier()
        {
            throw new NotImplementedException();
        }
        private bool CheckAttributeRange(FWCharacter pc, string attribute, int rangeLow, int rangeHigh)
        {
            bool pass = false;
            if (pc.GetFullAttributeScore(attribute) >= rangeLow
                && pc.GetFullAttributeScore(attribute) <= rangeHigh)
            {
                pass = true;
            }
            return pass;
        }
        public override bool StrikeCheck(BaseInteractiveObject io_source, long flags, long targ)
        {

            int source = io_source.RefId;
            int weapon = Io.RefId;
            if (Interactive.Instance.HasIO(source)
                && Interactive.Instance.HasIO(weapon))
            {
                // get specials, such as drain life or paralyze
                /*
                 * float drain_life = ARX_EQUIPMENT_GetSpecialValue(io_weapon, IO_SPECIAL_ELEM_DRAIN_LIFE);
                 * float paralyse = ARX_EQUIPMENT_GetSpecialValue(io_weapon, IO_SPECIAL_ELEM_PARALYZE);
                 */
                // check for a hit
                int mod = 0;
                int roll = Diceroller.Instance.RolldX(6);
                if (io_source.HasIOFlag(IoGlobals.IO_01_PC))
                {
                    FWCharacter pc = (FWCharacter)Io.PcData;
                    // -1 for INT 4-8
                    if (CheckAttributeRange(pc, "INT", 4, 8))
                    {
                        mod--;
                    }
                    // -1 for BRV 4-8
                    if (!Io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_BOW)
                        && CheckAttributeRange(pc, "BRV", 4, 8))
                    {
                        mod--;
                    }
                    // -1 for MAX END 4-8
                    if (!Io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_BOW)
                        && CheckAttributeRange(pc, "MEND", 4, 8))
                    {
                        mod--;
                    }
                    // -1 for being berserk
                    if (io_source.Script.GetLocalIntVariableValue("berserk_check") == HeroBase.BERSERK_BERSERK)
                    {
                        mod--;
                    }
                    // TODO -1 for being outnumbered per outnumbered
                    // -2 for INT 0-3
                    if (CheckAttributeRange(pc, "INT", 0, 3))
                    {
                        mod -= 2;
                    }
                    // -2 for BRV 0-3
                    if (!Io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_BOW)
                        && CheckAttributeRange(pc, "BRV", 0, 3))
                    {
                        mod -= 2;
                    }
                    // -2 for MEND 0-3
                    if (!Io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_BOW)
                        && CheckAttributeRange(pc, "MEND", 0, 3))
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
                    if (!Io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_BOW)
                        && CheckAttributeRange(pc, "INT", 14, 999))
                    {
                        mod++;
                    }
                    // +1 for MEND 14+
                    if (!Io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_BOW)
                        && CheckAttributeRange(pc, "MEND", 14, 999))
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
            }
            return false;
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
            }
        };
    }
}
