using RPGBase.Flyweights;
using RPGBase.Pooled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabLord.Constants
{
    public sealed class LabLordRace : IOClassification
    {
        /// <summary>
        /// flag indicating whether debugging is on.
        /// </summary>
        public bool debug = false;
        #region RACE CONSTANTS
        /// <summary>
        /// the list of in-game races.
        /// </summary>
        public static List<LabLordRace> Races;
        /// <summary>
        /// the Dwarf race.
        /// </summary>
        public const int RACE_DWARF = 0;
        /// <summary>
        /// the Elf race.
        /// </summary>
        public const int RACE_ELF = 1;
        /// <summary>
        /// the Gnome race.
        /// </summary>
        public const int RACE_GNOME = 2;
        /// <summary>
        /// the Halfling race.
        /// </summary>
        public const int RACE_HALFLING = 3;
        /// <summary>
        /// the Half-Elf race.
        /// </summary>
        public const int RACE_HALF_ELF = 4;
        /// <summary>
        /// the Half-Orc race.
        /// </summary>
        public const int RACE_HALF_ORC = 5;
        /// <summary>
        /// the Human race.
        /// </summary>
        public const int RACE_HUMAN = 6;
        /// <summary>
        /// the # of races.
        /// </summary>
        public const int NUM_RACES = 7;
        /// <summary>
        /// Flag indicating a race hasn't been assigned.
        /// </summary>
        public const int RACE_UNASSIGNED = -1;
        /// <summary>
        /// Static initializer.
        /// </summary>
        static LabLordRace()
        {
            Races = new List<LabLordRace>
            {
                new LabLordRace(RACE_DWARF,
                "Dwarf",
                "<b>Dwarves</b> are stout, short, bearded demi-humans who average a height of approximately 4 feet and weigh about 150 pounds. Due to their short height, dwarves cannot use two-handed weapons or longbows.",
                new Dictionary<int, int[,]>
                {
                    {
                        LabLordAbility.ABILITY_STR, new int[,] { {8,18 }, {8,17} }
                    },
                    {
                        LabLordAbility.ABILITY_DEX, new int[,] { {3,17 }, {3,17} }
                    },
                    {
                        LabLordAbility.ABILITY_CON, new int[,] { {12,19 }, {12,19} }
                    },
                    {
                        LabLordAbility.ABILITY_INT, new int[,] { {3,18 }, {3,18} }
                    },
                    {
                        LabLordAbility.ABILITY_WIS, new int[,] { {3,18 }, {3,18 } }
                    },
                    {
                        LabLordAbility.ABILITY_CHA, new int[,] { {3,16 }, {3,16} }
                    }
                }),
                new LabLordRace(RACE_ELF,
                "Elf",
                "<b>Elves</b> have pointed ears, and are thin, fey beings. They typically weigh about 120 pounds and are between 5 and 5 1⁄2 feet tall.",
                new Dictionary<int, int[,]>
                {
                    {
                        LabLordAbility.ABILITY_STR, new int[,] { {3,18 }, {3,16} }
                    },
                    {
                        LabLordAbility.ABILITY_DEX, new int[,] { {7,19 }, {7, 19} }
                    },
                    {
                        LabLordAbility.ABILITY_CON, new int[,] { {6,18 }, {6, 18} }
                    },
                    {
                        LabLordAbility.ABILITY_INT, new int[,] { {8,18 }, {8, 18} }
                    },
                    {
                        LabLordAbility.ABILITY_WIS, new int[,] { {3,18 }, {3, 18} }
                    },
                    {
                        LabLordAbility.ABILITY_CHA, new int[,] { {3,18 }, {3, 18} }
                    }
                }),
                new LabLordRace(RACE_GNOME,
                "Gnome",
                "<b>Gnomes</b> are cousins to dwarves, and share many of their idiosyncrasies. They have a wide range of appearances, and average about 4 feet tall like dwarves; they tend to be much more slender, averaging 100 pounds. Gnomes may not use large and two-handed weapons, but they may use weapon and armor as indicated by class.",
                new Dictionary<int, int[,]>
                {
                    {
                        LabLordAbility.ABILITY_STR, new int[,] { {6,18 }, {6,15} }
                    },
                    {
                        LabLordAbility.ABILITY_DEX, new int[,] { {3,18 }, {3, 18} }
                    },
                    {
                        LabLordAbility.ABILITY_CON, new int[,] { {8,18 }, {8, 18} }
                    },
                    {
                        LabLordAbility.ABILITY_INT, new int[,] { {7,18 }, {7, 18} }
                    },
                    {
                        LabLordAbility.ABILITY_WIS, new int[,] { {3,18 }, {3, 18} }
                    },
                    {
                        LabLordAbility.ABILITY_CHA, new int[,] { {8,18 }, {8, 18} }
                    }
                }),
                new LabLordRace(RACE_HALFLING,
                "Halfling",
                "<b>Halflings</b> are even smaller than dwarves, being about 60 pounds and only attaining a height of around 3 feet. Like dwarves, halflings may not use large and two-handed weapons, but may use any other weapon and armor as indicated by class.",
                new Dictionary<int, int[,]>
                {
                    {
                        LabLordAbility.ABILITY_STR, new int[,] { {6,17 }, {6,14} }
                    },
                    {
                        LabLordAbility.ABILITY_DEX, new int[,] { {8,18 }, {8, 18} }
                    },
                    {
                        LabLordAbility.ABILITY_CON, new int[,] { {10,19},{10, 19} }
                    },
                    {
                        LabLordAbility.ABILITY_INT, new int[,] { {6,18 }, {6, 18} }
                    },
                    {
                        LabLordAbility.ABILITY_WIS, new int[,] { {3,17 }, {3, 17} }
                    },
                    {
                        LabLordAbility.ABILITY_CHA, new int[,] { {3,18 }, {3, 18} }
                    }
                }),
                new LabLordRace(RACE_HALF_ELF,
                "Half-Elf",
                "<b>Half-elves</b> are the result of the union of human and elf, and as such they seldom fit into either society. They are slight of build, averaging 150 pounds with an average height of 5 1⁄2 feet. They have pointed ears, and have inherited a love of nature from their elven parent.",
                new Dictionary<int, int[,]>
                {
                    {
                        LabLordAbility.ABILITY_STR, new int[,] { {3,18 }, {3,17} }
                    },
                    {
                        LabLordAbility.ABILITY_DEX, new int[,] { {6,18 }, {6, 18} }
                    },
                    {
                        LabLordAbility.ABILITY_CON, new int[,] { {6,18 }, {6, 18} }
                    },
                    {
                        LabLordAbility.ABILITY_INT, new int[,] { {4,18 }, {4, 18} }
                    },
                    {
                        LabLordAbility.ABILITY_WIS, new int[,] { {3,18 }, {3, 18} }
                    },
                    {
                        LabLordAbility.ABILITY_CHA, new int[,] { {3,18 }, {3, 18} }
                    }
                }),
                new LabLordRace(RACE_HALF_ORC,
                "Half-Orc",
                "<b>Half-Orcs</b> result from orc and human matings. Orcs will breed with nearly any humanoid, and are fertile beings. The majority of orcish cross-breeds are nearly indistinguishable from orcs in appearance and behavior.",
                new Dictionary<int, int[,]>
                {
                    {
                        LabLordAbility.ABILITY_STR, new int[,] { {6,18 }, {6,18} }
                    },
                    {
                        LabLordAbility.ABILITY_DEX, new int[,] { {3,17 }, {3, 17} }
                    },
                    {
                        LabLordAbility.ABILITY_CON, new int[,] { {13,19},{13, 19} }
                    },
                    {
                        LabLordAbility.ABILITY_INT, new int[,] { {3,17 }, {3, 17} }
                    },
                    {
                        LabLordAbility.ABILITY_WIS, new int[,] { {3,14 }, {3, 14} }
                    },
                    {
                        LabLordAbility.ABILITY_CHA, new int[,] { {3,12 }, {3, 12} }
                    }
                }),
                new LabLordRace(RACE_HUMAN,
                "Human",
                "<b>Humans</b> come in a wide range of appearances, and are versatile beings.This very versatility grants them the ability to choose any class, with no level restrictions. Humans are generally the most common race in a fantasy world, and they serve as the baseline from which all of the demi-human races are compared.",
                new Dictionary<int, int[,]>
                {
                    {
                        LabLordAbility.ABILITY_STR, new int[,] { {3,18 }, {3,18} }
                    },
                    {
                        LabLordAbility.ABILITY_DEX, new int[,] { {3,18 }, {3, 18} }
                    },
                    {
                        LabLordAbility.ABILITY_CON, new int[,] { {3,18 }, {3, 18} }
                    },
                    {
                        LabLordAbility.ABILITY_INT, new int[,] { {3,18 }, {3, 18} }
                    },
                    {
                        LabLordAbility.ABILITY_WIS, new int[,] { {3,18 }, {3, 18} }
                    },
                    {
                        LabLordAbility.ABILITY_CHA, new int[,] { {3,18 }, {3, 18} }
                    }
                })
            };
            // DWARF CON +1 CHA -1
            Races[RACE_DWARF].Modifiers[LabLordGlobals.EQUIP_ELEMENT_CON] = new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Dwarf");
            Races[RACE_DWARF].Modifiers[LabLordGlobals.EQUIP_ELEMENT_CHA] = new EquipmentItemModifier(-1, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Dwarf");
            // ELF DEX +1 CON -1
            Races[RACE_ELF].Modifiers[LabLordGlobals.EQUIP_ELEMENT_DEX] = new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Elf");
            Races[RACE_ELF].Modifiers[LabLordGlobals.EQUIP_ELEMENT_CON] = new EquipmentItemModifier(-1, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Elf");
            // HAFLING STR -1 DEX +1
            Races[RACE_HALFLING].Modifiers[LabLordGlobals.EQUIP_ELEMENT_STR] = new EquipmentItemModifier(-1, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Halfling");
            Races[RACE_HALFLING].Modifiers[LabLordGlobals.EQUIP_ELEMENT_DEX] = new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Halfling");
            // HALF-ORC STR +1 CON +1 CHA -2
            Races[RACE_HALF_ORC].Modifiers[LabLordGlobals.EQUIP_ELEMENT_STR] = new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Half-Orc");
            Races[RACE_HALF_ORC].Modifiers[LabLordGlobals.EQUIP_ELEMENT_CON] = new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Half-Orc");
            Races[RACE_HALF_ORC].Modifiers[LabLordGlobals.EQUIP_ELEMENT_CHA] = new EquipmentItemModifier(-2, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Half-Orc");
            // DWARF +2 Breath, +4 Poison, +4 Petrify/Paralyze, +3 Wands, +4 Spells
            Races[RACE_DWARF].Modifiers[LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH] = new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Dwarf");
            Races[RACE_DWARF].Modifiers[LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON] = new EquipmentItemModifier(4, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Dwarf");
            Races[RACE_DWARF].Modifiers[LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PETRIFY] = new EquipmentItemModifier(4, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Dwarf");
            Races[RACE_DWARF].Modifiers[LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PARALYZE] = new EquipmentItemModifier(4, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Dwarf");
            Races[RACE_DWARF].Modifiers[LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS] = new EquipmentItemModifier(3, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Dwarf");
            Races[RACE_DWARF].Modifiers[LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS] = new EquipmentItemModifier(4, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Dwarf");
            // GNOME +2 Breath, +4 Poison, +4 Petrify/Paralyze, +1 Wands, +2 Spells
            Races[RACE_GNOME].Modifiers[LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH] = new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Gnome");
            Races[RACE_GNOME].Modifiers[LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON] = new EquipmentItemModifier(4, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Gnome");
            Races[RACE_GNOME].Modifiers[LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PETRIFY] = new EquipmentItemModifier(4, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Gnome");
            Races[RACE_GNOME].Modifiers[LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PARALYZE] = new EquipmentItemModifier(4, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Gnome");
            Races[RACE_GNOME].Modifiers[LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS] = new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Gnome");
            Races[RACE_GNOME].Modifiers[LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS] = new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Gnome");
            // HAFLING +2 Breath, +4 Poison, +4 Petrify/Paralyze, +3 Wands, +4 Spells
            Races[RACE_GNOME].Modifiers[LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH] = new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Halfling");
            Races[RACE_GNOME].Modifiers[LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON] = new EquipmentItemModifier(4, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Halfling");
            Races[RACE_GNOME].Modifiers[LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PETRIFY] = new EquipmentItemModifier(4, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Halfling");
            Races[RACE_GNOME].Modifiers[LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PARALYZE] = new EquipmentItemModifier(4, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Halfling");
            Races[RACE_GNOME].Modifiers[LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS] = new EquipmentItemModifier(3, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Halfling");
            Races[RACE_GNOME].Modifiers[LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS] = new EquipmentItemModifier(4, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Halfling");
            // DWARF +7 Pick Locks, +10 F/R Traps, -10 Climb
            Races[RACE_DWARF].Modifiers[LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_LOCKS] = new EquipmentItemModifier(7, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Dwarf");
            Races[RACE_DWARF].Modifiers[LabLordGlobals.EQUIP_ELEMENT_THIEF_FIND_REMOVE_TRAPS] = new EquipmentItemModifier(10, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Dwarf");
            Races[RACE_DWARF].Modifiers[LabLordGlobals.EQUIP_ELEMENT_THIEF_CLIMB_WALLS] = new EquipmentItemModifier(-10, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Dwarf");
            // ELF -5 Pick Locks, +5 Pick Pockets, +7 Move, +10 Hide, +1 Hear
            Races[RACE_ELF].Modifiers[LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_LOCKS] = new EquipmentItemModifier(-5, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Elf");
            Races[RACE_ELF].Modifiers[LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_POCKETS] = new EquipmentItemModifier(5, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Elf");
            Races[RACE_ELF].Modifiers[LabLordGlobals.EQUIP_ELEMENT_THIEF_MOVE_SILENTLY] = new EquipmentItemModifier(7, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Elf");
            Races[RACE_ELF].Modifiers[LabLordGlobals.EQUIP_ELEMENT_THIEF_HIDE_SHADOWS] = new EquipmentItemModifier(10, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Elf");
            Races[RACE_ELF].Modifiers[LabLordGlobals.EQUIP_ELEMENT_THIEF_HEAR_NOISE] = new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Elf");
            // GNOME +5 Pick Locks, +7 F/R Traps, +5 Move, -15 Climb, +5 Hide
            Races[RACE_GNOME].Modifiers[LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_LOCKS] = new EquipmentItemModifier(5, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Gnome");
            Races[RACE_GNOME].Modifiers[LabLordGlobals.EQUIP_ELEMENT_THIEF_FIND_REMOVE_TRAPS] = new EquipmentItemModifier(7, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Gnome");
            Races[RACE_GNOME].Modifiers[LabLordGlobals.EQUIP_ELEMENT_THIEF_MOVE_SILENTLY] = new EquipmentItemModifier(5, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Gnome");
            Races[RACE_GNOME].Modifiers[LabLordGlobals.EQUIP_ELEMENT_THIEF_CLIMB_WALLS] = new EquipmentItemModifier(-15, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Gnome");
            Races[RACE_GNOME].Modifiers[LabLordGlobals.EQUIP_ELEMENT_THIEF_HIDE_SHADOWS] = new EquipmentItemModifier(5, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Gnome");
            // HALFLING +5 Locks, +5 F/R Traps, +5 Pockets, +10 Move, -15 Climb, +10 Hide
            Races[RACE_HALFLING].Modifiers[LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_LOCKS] = new EquipmentItemModifier(5, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Halfling");
            Races[RACE_HALFLING].Modifiers[LabLordGlobals.EQUIP_ELEMENT_THIEF_FIND_REMOVE_TRAPS] = new EquipmentItemModifier(5, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Halfling");
            Races[RACE_HALFLING].Modifiers[LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_POCKETS] = new EquipmentItemModifier(5, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Halfling");
            Races[RACE_HALFLING].Modifiers[LabLordGlobals.EQUIP_ELEMENT_THIEF_MOVE_SILENTLY] = new EquipmentItemModifier(10, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Halfling");
            Races[RACE_HALFLING].Modifiers[LabLordGlobals.EQUIP_ELEMENT_THIEF_CLIMB_WALLS] = new EquipmentItemModifier(-15, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Halfling");
            Races[RACE_HALFLING].Modifiers[LabLordGlobals.EQUIP_ELEMENT_THIEF_HIDE_SHADOWS] = new EquipmentItemModifier(10, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Halfling");
            // HALF-ELF +10 Pockets, +5 Hide
            Races[RACE_HALF_ELF].Modifiers[LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_POCKETS] = new EquipmentItemModifier(10, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Half-Elf");
            Races[RACE_HALF_ELF].Modifiers[LabLordGlobals.EQUIP_ELEMENT_THIEF_HIDE_SHADOWS] = new EquipmentItemModifier(5, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Half-Elf");
            // HALF-ORC +5 Locks, +5 Traps, -5 Pockets, +5 Climb
            Races[RACE_HALF_ORC].Modifiers[LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_LOCKS] = new EquipmentItemModifier(5, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Half-Orc");
            Races[RACE_HALF_ORC].Modifiers[LabLordGlobals.EQUIP_ELEMENT_THIEF_FIND_REMOVE_TRAPS] = new EquipmentItemModifier(5, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Half-Orc");
            Races[RACE_HALF_ORC].Modifiers[LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_POCKETS] = new EquipmentItemModifier(-5, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Half-Orc");
            Races[RACE_HALF_ORC].Modifiers[LabLordGlobals.EQUIP_ELEMENT_THIEF_CLIMB_WALLS] = new EquipmentItemModifier(5, false, false, 0, LabLordGlobals.MODIFIER_SRC_RACE, "Half-Orc");
            // PLURALS
            Races[RACE_DWARF].Plural = "Dwarves";
            Races[RACE_ELF].Plural = "Elves";
            Races[RACE_GNOME].Plural = "Gnomes";
            Races[RACE_HALFLING].Plural = "Halflings";
            Races[RACE_HALF_ELF].Plural = "Half-Elves";
            Races[RACE_HALF_ORC].Plural = "Half-Orcs";
            Races[RACE_HUMAN].Plural = "Humans";
        }
        #endregion
        /// <summary>
        /// The field defining the list of ability requirements.
        /// </summary>
        private Dictionary<int, int[,]> requirements;
        /// <summary>
        /// The property defining the list of ability requirements.
        /// </summary>
        public Dictionary<int, int[,]> Requirements { get { return requirements; } }
        /// <summary>
        /// The list of modifiers applied for the specific race.
        /// </summary>
        public EquipmentItemModifier[] Modifiers = new EquipmentItemModifier[LabLordGlobals.NUM_ELEMENTS];
        /// <summary>
        /// the plural form of the race.
        /// </summary>
        public string Plural { get; private set; }
        private string StandardTooltip()
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append(Description);
            sb.Append("\n");
            bool indented = false;
            bool atLeastOne = false;
            for (int i = 0, li = LabLordGlobals.EQUIP_ELEMENT_CHA; i <= li; i++)
            {
                if (Modifiers[i] != null
                    && Modifiers[i].Value != 0f)
                {
                    atLeastOne = true;
                    break;
                }
            }
            if (atLeastOne)
            {
                sb.Append("\nAbility Modifiers:\t");
                string indent = "\t\t\t\t\t\t\t";
                string redStart = "<color=red>", redStop = "</color>";
                for (int i = 0, li = LabLordGlobals.EQUIP_ELEMENT_CHA; i <= li; i++)
                {
                    if (Modifiers[i] == null)
                    {
                        continue;
                    }
                    if (Modifiers[i].Value == 0f)
                    {
                        continue;
                    }
                    if (indented)
                    {
                        sb.Append(indent);
                    }
                    int val = (int)Modifiers[i].Value;
                    sb.Append(LabLordAbility.Abilities[i].Abbr);
                    sb.Append(" ");
                    if (val > 0)
                    {
                        sb.Append("+");
                    }
                    if (val < 0)
                    {
                        sb.Append(redStart);
                    }
                    sb.Append(val);
                    if (val < 0)
                    {
                        sb.Append(redStop);
                    }
                    sb.Append("\n");
                    indented = true;
                }
            }
            atLeastOne = false;
            for (int i = LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH, li = LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS; i <= li; i++)
            {
                if (Modifiers[i] != null
                    && Modifiers[i].Value != 0f)
                {
                    atLeastOne = true;
                    break;
                }
            }
            if (atLeastOne)
            {
                sb.Append("\nSave Bonuses:\t");
                indented = false;
                string indent = "\t\t\t\t\t\t";
                string redStart = "<color=red>", redStop = "</color>";
                for (int i = LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH, li = LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS; i <= li; i++)
                {
                    if (Modifiers[i] == null)
                    {
                        continue;
                    }
                    if (Modifiers[i].Value == 0f)
                    {
                        continue;
                    }
                    if (indented)
                    {
                        sb.Append(indent);
                    }
                    int val = (int)Modifiers[i].Value;
                    if (val > 0)
                    {
                        sb.Append("+");
                    }
                    if (val < 0)
                    {
                        sb.Append(redStart);
                    }
                    sb.Append(val);
                    if (val < 0)
                    {
                        sb.Append(redStop);
                    }
                    sb.Append(" ");
                    sb.Append(LabLordSaveThrow.SAVE_SHORT_TITLES[i - LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH]);
                    sb.Append("\n");
                    indented = true;
                }
            }
            atLeastOne = false;
            for (int i = LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_LOCKS, li = LabLordGlobals.EQUIP_ELEMENT_THIEF_HEAR_NOISE; i <= li; i++)
            {
                if (Modifiers[i] != null
                    && Modifiers[i].Value != 0f)
                {
                    atLeastOne = true;
                    break;
                }
            }
            if (atLeastOne)
            {
                sb.Append("\nThief Skill Adjustments:\n");
                string redStart = "<color=red>", redStop = "</color>";
                for (int i = LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_LOCKS, li = LabLordGlobals.EQUIP_ELEMENT_THIEF_HEAR_NOISE; i <= li; i++)
                {
                    if (Modifiers[i] == null)
                    {
                        continue;
                    }
                    if (Modifiers[i].Value == 0f)
                    {
                        continue;
                    }
                    sb.Append(LabLordThiefSkills.THIEF_SKILL_TITLES[i - LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_LOCKS]);
                    sb.Append(LabLordThiefSkills.TAB_LIST[i - LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_LOCKS]);
                    int val = (int)Modifiers[i].Value;
                    if (val > 0)
                    {
                        sb.Append("+");
                    }
                    if (val < 0)
                    {
                        sb.Append(redStart);
                    }
                    sb.Append(val);
                    sb.Append("%");
                    if (val < 0)
                    {
                        sb.Append(redStop);
                    }
                    sb.Append("\n");
                }
            }
            string s = sb.ToString();
            sb.ReturnToPool();
            return s;
        }
        private string RaceUnavailableTooltip(int gender, int[] abilities)
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            PooledStringBuilder sb2 = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append(Title);
            sb.Append(" Unavailable\n\n");
            List<string> list = new List<string>();
            for (int ability = 0, li = abilities.Length; ability < li; ability++)
            {
                int score = abilities[ability];
                int[,] minmax = requirements[ability];
                int min = minmax[gender, 0], max = minmax[gender, 1];
                if (score < min)
                {
                    sb2.Append(LabLordAbility.Abilities[ability].Abbr);
                    sb2.Append(" more than ");
                    sb2.Append(min - 1);
                    list.Add(sb2.ToString());
                    sb2.Length = 0;
                }
                else if (score > max)
                {
                    sb2.Append(LabLordAbility.Abilities[ability].Abbr);
                    sb2.Append(" less than ");
                    sb2.Append(max + 1);
                    list.Add(sb2.ToString());
                    sb2.Length = 0;
                }
            }
            if (list.Count > 0)
            {
                sb.Append("Requirements:\n");
                for (int i = 0, li = list.Count; i < li; i++)
                {

                    sb.Append(list[i]);
                    sb.Append("\n");
                }
            }
            else
            {
                sb.Append("No qualifying Classes available");
            }
            string s = sb.ToString();
            sb.ReturnToPool();
            sb2.ReturnToPool();
            return s;
        }
        public string GetTooltipDescription(int gender, int[] abilities, bool available)
        {
            if (debug)
            {
                UnityEngine.Debug.Log(Title + " available? " + available);
            }
            string s = "";
            if (available)
            {
                s = StandardTooltip();
            }
            else
            {
                s = RaceUnavailableTooltip(gender, abilities);
            }
            return s;
        }
        /// <summary>
        /// Hidden constructor.
        /// </summary>
        private LabLordRace(int c, string t, string d, Dictionary<int, int[,]> dict) : base(c, t, d)
        {
            requirements = dict;
        }
    }
}
