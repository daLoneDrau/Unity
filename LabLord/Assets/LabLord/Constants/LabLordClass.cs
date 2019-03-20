using RPGBase.Flyweights;
using RPGBase.Pooled;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabLord.Constants
{
    public sealed class LabLordClass : IOClassification
    {
        /// <summary>
        /// flag indicating whether debugging is on.
        /// </summary>
        public bool debug;
        #region CHARACTER CLASS CONSTANTS
        /// <summary>
        /// the list of in-game classes.
        /// </summary>
        public static List<LabLordClass> Classes;
        /// <summary>
        /// the Assassin class.
        /// </summary>
        public const int CLASS_ASSASSIN = 0;
        /// <summary>
        /// the Cleric class.
        /// </summary>
        public const int CLASS_CLERIC = 1;
        /// <summary>
        /// the Druid class.
        /// </summary>
        public const int CLASS_DRUID = 2;
        /// <summary>
        /// the Fighter class.
        /// </summary>
        public const int CLASS_FIGHTER = 3;
        /// <summary>
        /// the Illusionist class.
        /// </summary>
        public const int CLASS_ILLUSIONIST = 4;
        /// <summary>
        /// the Mage class.
        /// </summary>
        public const int CLASS_MAGIC_USER = 5;
        /// <summary>
        /// the Monk class.
        /// </summary>
        public const int CLASS_MONK = 6;
        /// <summary>
        /// the Paladin class.
        /// </summary>
        public const int CLASS_PALADIN = 7;
        /// <summary>
        /// the Ranger class.
        /// </summary>
        public const int CLASS_RANGER = 8;
        /// <summary>
        /// the Thief class.
        /// </summary>
        public const int CLASS_THIEF = 9;
        /// <summary>
        /// the total # of classes.
        /// </summary>
        public const int NUM_CLASSES = 10;
        /// <summary>
        /// Static initializer.
        /// </summary>
        static LabLordClass()
        {
            Classes = new List<LabLordClass>
            {
                new LabLordClass(CLASS_ASSASSIN,
                "Assassin",
                "The <b>Assassin</b> is a specialized variation of the thief, with the primary objective of killing for hire. Assassins may also be hired as spies. An assassin will usually belong to an assassin's guild from the character's local town.",
                new Dictionary<int, int>
                {
                    { LabLordAbility.ABILITY_STR, 12 },
                    { LabLordAbility.ABILITY_DEX, 12 },
                    { LabLordAbility.ABILITY_INT, 12 }
                },
                4),
                new LabLordClass(CLASS_CLERIC,
                "Cleric",
                "<b>Clerics</b> have pledged their lives to serve a deity. To this end, they conduct their lives in a way to further the desires and will of their gods or goddesses. Clerics may use divine energy in the form of spells, which are granted through prayer and worship. The power and number of cleric spells available to a character are determined by level. Clerics are also trained to fight, and they should be thought of not as passive priests but as fighting holy crusaders. If a cleric ever falls from favor, due to violating the beliefs of his god or breaking the rules of his clergy, the god may impose penalties upon the cleric.",
                new Dictionary<int, int>
                {
                    { LabLordAbility.ABILITY_WIS, 12 }
                },
                6),
                new LabLordClass(CLASS_DRUID,
                "Druid",
                "<b>Druids</b> are a secretive subclass of clerics. Their ambitions and methods are often not understood, which makes them objects of both fascination and fear. Much like clerics, druids can be seen as \"holy warriors\", but their allegiance is not to a typical god. Rather, they pay homage to nature. The sun, the moon, the earth, the elements, and all things associated with these forces are their gods. As a consequence, druids are not bound by typical concepts of \"good\" or \"evil\", for nature does not exist to adhere to human moral concepts. Thus, all druids must be neutrally aligned. Druids are dedicated to protecting the balance of nature; sometimes protecting that balance requires acts others might view as \"evil\" or \"good\".",
                new Dictionary<int, int>
                {
                    { LabLordAbility.ABILITY_WIS, 12 },
                    { LabLordAbility.ABILITY_CHA, 15 }
                },
                6),
                new LabLordClass(CLASS_FIGHTER,
                "Fighter",
                "<b>Fighters</b>, as their name implies, are exclusively trained in the arts of combat and war. They are specialists at dealing physical blows. Unlike other classes, fighters are particularly burdened in a group of adventurers because they are tougher and must take the lead to defend others. Fighters can use any weapons and armor.",
                new Dictionary<int, int>
                {
                    { LabLordAbility.ABILITY_STR, 12 }
                },
                8),
                new LabLordClass(CLASS_ILLUSIONIST,
                "Illusionist",
                "<b>Illusionists</b> are a specialized form of magic-user. They have access to some of the same spells, but also an array of specialist spells designed to confuse the senses and deceive the unwary. Illusionists may use many of the same magic items available to all characters.",
                new Dictionary<int, int>
                {
                    { LabLordAbility.ABILITY_DEX, 16 },
                    { LabLordAbility.ABILITY_INT, 15 }
                },
                4),
                new LabLordClass(CLASS_MAGIC_USER,
                "Mage",
                "Sometimes called wizards, warlocks, or witches, <b>Mages</b> study arcane secrets and cast spells. Mages are able to cast a greater number of increasingly more powerful spells as they advance in level. However, they are limited in their choice of weapons, as they are only able to use small weapons such as a dagger. They are unable to use shields or wear any kind of armor. For these reasons, mages are weak at low levels, and in an adventuring group they should be protected.",
                new Dictionary<int, int>
                {
                    { LabLordAbility.ABILITY_INT, 12 }
                },
                4),
                new LabLordClass(CLASS_MONK,
                "Monk",
                "<b>Monks</b> are a subclass of clerics; however, while clerics look outward for wisdom monks seek inner enlightenment. They do not learn or cast spells. Rather, they finely hone themselves to their full potential through training of both mind and body. As a result, they are able to perform feats unattainable by other classes.\nMonks may only have small amounts of money, like paladins. They may wear no armor. However, they may use any weapon and attack as thieves.",
                new Dictionary<int, int>
                {
                    { LabLordAbility.ABILITY_STR, 12 },
                    { LabLordAbility.ABILITY_DEX, 15 },
                    { LabLordAbility.ABILITY_WIS, 15 }
                },
                4),
                new LabLordClass(CLASS_PALADIN,
                "Paladin",
                "<b>Paladins</b> are a type of fighter that adheres to a strict moral code. They must never commit morally questionable, or evil, acts. Should a paladin knowingly act in a chaotic way, only confession and paying penance to a cleric of 7th level or higher will remove the mark of the sin. However, committing an evil act is unforgivable, and a paladin immediately loses all special class abilities and becomes an ordinary fighter of equal level, with the lowest possible experience points.",
                new Dictionary<int, int>
                {
                    { LabLordAbility.ABILITY_STR, 12 },
                    { LabLordAbility.ABILITY_INT, 9 },
                    { LabLordAbility.ABILITY_WIS, 13 },
                    { LabLordAbility.ABILITY_CHA, 17 }
                },
                8),
                new LabLordClass(CLASS_RANGER,
                "Ranger",
                "<b>Rangers</b> are a sub-class of the fighter who is a specialist of wilderness survival, whether that wilderness be above or below ground. They adhere to their own sort of morals, such that they may be lawful, chaotic, or neutral with their larger world views, which may not reflect their disposition to other beings. In this regard, they have some similarity to the druids.",
                new Dictionary<int, int>
                {
                    { LabLordAbility.ABILITY_STR, 12 },
                    { LabLordAbility.ABILITY_INT, 12 },
                    { LabLordAbility.ABILITY_CON, 15 },
                    { LabLordAbility.ABILITY_WIS, 12 }
                },
                6),
                new LabLordClass(CLASS_THIEF,
                "Thief",
                "<b>Thieves</b> have a range of unique skills associated with their profession that make them very handy companions in adventures. However, thieves can be a bit shady and they sometimes are not as trustworthy as other classes. A thief will usually belong to a <b>Thieves Guild</b> from the character's local town, where they can seek shelter and information between adventures.",
                new Dictionary<int, int>
                {
                    { LabLordAbility.ABILITY_DEX, 12 }
                },
                4)
            };
            // PRIME REQUISITES
            Classes[CLASS_CLERIC].PrimeRequisites = new int[] { LabLordAbility.ABILITY_WIS };
            Classes[CLASS_DRUID].PrimeRequisites = new int[] { LabLordAbility.ABILITY_WIS, LabLordAbility.ABILITY_CHA };
            Classes[CLASS_FIGHTER].PrimeRequisites = new int[] { LabLordAbility.ABILITY_STR };
            Classes[CLASS_MAGIC_USER].PrimeRequisites = new int[] { LabLordAbility.ABILITY_INT };
            Classes[CLASS_PALADIN].PrimeRequisites = new int[] { LabLordAbility.ABILITY_STR, LabLordAbility.ABILITY_WIS };
            Classes[CLASS_RANGER].PrimeRequisites = new int[] { LabLordAbility.ABILITY_STR, LabLordAbility.ABILITY_INT, LabLordAbility.ABILITY_WIS };
            Classes[CLASS_THIEF].PrimeRequisites = new int[] { LabLordAbility.ABILITY_DEX };
            #region MAX LEVEL
            Classes[CLASS_ASSASSIN].MaxLevelForRace = new Dictionary<int, int> {
                { LabLordRace.RACE_DWARF, 9 },
                { LabLordRace.RACE_ELF, 10 },
                { LabLordRace.RACE_GNOME, 8 },
                { LabLordRace.RACE_HALFLING, 0 },
                { LabLordRace.RACE_HALF_ELF, 11 },
                { LabLordRace.RACE_HALF_ORC, 15 },
                { LabLordRace.RACE_HUMAN, 15 }
            };
            Classes[CLASS_CLERIC].MaxLevelForRace = new Dictionary<int, int> {
                { LabLordRace.RACE_DWARF, 8 },
                { LabLordRace.RACE_ELF, 7 },
                { LabLordRace.RACE_GNOME, 7 },
                { LabLordRace.RACE_HALFLING, 0 },
                { LabLordRace.RACE_HALF_ELF, 5 },
                { LabLordRace.RACE_HALF_ORC, 4 },
                { LabLordRace.RACE_HUMAN, 999 }
            };
            Classes[CLASS_DRUID].MaxLevelForRace = new Dictionary<int, int> {
                { LabLordRace.RACE_DWARF, 0 },
                { LabLordRace.RACE_ELF, 0 },
                { LabLordRace.RACE_GNOME, 0 },
                { LabLordRace.RACE_HALFLING, 0 },
                { LabLordRace.RACE_HALF_ELF, 0 },
                { LabLordRace.RACE_HALF_ORC, 0 },
                { LabLordRace.RACE_HUMAN, 999 }
            };
            Classes[CLASS_FIGHTER].MaxLevelForRace = new Dictionary<int, int> {
                { LabLordRace.RACE_DWARF, 9 },
                { LabLordRace.RACE_ELF, 10 },
                { LabLordRace.RACE_GNOME, 6 },
                { LabLordRace.RACE_HALFLING, 6 },
                { LabLordRace.RACE_HALF_ELF, 12 },
                { LabLordRace.RACE_HALF_ORC, 12 },
                { LabLordRace.RACE_HUMAN, 999 }
            };
            Classes[CLASS_ILLUSIONIST].MaxLevelForRace = new Dictionary<int, int> {
                { LabLordRace.RACE_DWARF, 0 },
                { LabLordRace.RACE_ELF, 0 },
                { LabLordRace.RACE_GNOME, 7 },
                { LabLordRace.RACE_HALFLING, 0 },
                { LabLordRace.RACE_HALF_ELF, 0 },
                { LabLordRace.RACE_HALF_ORC, 0 },
                { LabLordRace.RACE_HUMAN, 999 }
            };
            Classes[CLASS_MAGIC_USER].MaxLevelForRace = new Dictionary<int, int> {
                { LabLordRace.RACE_DWARF, 0 },
                { LabLordRace.RACE_ELF, 11 },
                { LabLordRace.RACE_GNOME, 0 },
                { LabLordRace.RACE_HALFLING, 0 },
                { LabLordRace.RACE_HALF_ELF, 10 },
                { LabLordRace.RACE_HALF_ORC, 0 },
                { LabLordRace.RACE_HUMAN, 999 }
            };
            Classes[CLASS_MONK].MaxLevelForRace = new Dictionary<int, int> {
                { LabLordRace.RACE_DWARF, 0 },
                { LabLordRace.RACE_ELF, 0 },
                { LabLordRace.RACE_GNOME, 0 },
                { LabLordRace.RACE_HALFLING, 0 },
                { LabLordRace.RACE_HALF_ELF, 0 },
                { LabLordRace.RACE_HALF_ORC, 0 },
                { LabLordRace.RACE_HUMAN, 999 }
            };
            Classes[CLASS_PALADIN].MaxLevelForRace = new Dictionary<int, int> {
                { LabLordRace.RACE_DWARF, 0 },
                { LabLordRace.RACE_ELF, 0 },
                { LabLordRace.RACE_GNOME, 0 },
                { LabLordRace.RACE_HALFLING, 0 },
                { LabLordRace.RACE_HALF_ELF, 0 },
                { LabLordRace.RACE_HALF_ORC, 0 },
                { LabLordRace.RACE_HUMAN, 999 }
            };
            Classes[CLASS_RANGER].MaxLevelForRace = new Dictionary<int, int> {
                { LabLordRace.RACE_DWARF, 0 },
                { LabLordRace.RACE_ELF, 0 },
                { LabLordRace.RACE_GNOME, 0 },
                { LabLordRace.RACE_HALFLING, 0 },
                { LabLordRace.RACE_HALF_ELF, 8 },
                { LabLordRace.RACE_HALF_ORC, 0 },
                { LabLordRace.RACE_HUMAN, 999 }
            };
            Classes[CLASS_THIEF].MaxLevelForRace = new Dictionary<int, int> {
                { LabLordRace.RACE_DWARF, 12 },
                { LabLordRace.RACE_ELF, 12 },
                { LabLordRace.RACE_GNOME, 12 },
                { LabLordRace.RACE_HALFLING, 14 },
                { LabLordRace.RACE_HALF_ELF, 12 },
                { LabLordRace.RACE_HALF_ORC, 12 },
                { LabLordRace.RACE_HUMAN, 999 }
            };
            #endregion
            #region XP PER LEVEL
            Classes[CLASS_ASSASSIN].LevelRequirements = new Dictionary<int, int> {
                { 1, 0 },
                { 2, 1501 },
                { 3, 3001 },
                { 4, 6001 },
                { 5, 12001 },
                { 6, 24001 },
                { 7, 48001 },
                { 8, 96001 },
                { 9, 192000 },
                { 10, 332000 },
                { 11, 472000 },
                { 12, 612000 },
                { 13, 762000 },
                { 14, 902000 },
                { 15, 1042000 }
            };
            Classes[CLASS_CLERIC].LevelRequirements = new Dictionary<int, int> {
                { 1, 0 },
                { 2, 1565 },
                { 3, 3125 },
                { 4, 6251 },
                { 5, 12501 },
                { 6, 25001 },
                { 7, 50001 },
                { 8, 100001 },
                { 9, 200001 },
                { 10, 300001 },
                { 11, 400001 },
                { 12, 500001 },
                { 13, 600001 },
                { 14, 700001 },
                { 15, 800001 },
                { 16, 900001 },
                { 17, 1000001 },
                { 18, 1100001 },
                { 19, 1200001 },
                { 20, 1300001 },
            };
            Classes[CLASS_DRUID].LevelRequirements = new Dictionary<int, int> {
                { 1, 0 },
                { 2, 2065 },
                { 3, 4125 },
                { 4, 7751 },
                { 5, 12501 },
                { 6, 20001 },
                { 7, 40001 },
                { 8, 60001 },
                { 9, 90001 },
                { 10, 150001 },
                { 11, 200001 },
                { 12, 300001 },
                { 13, 750001 },
                { 14, 1500001 }
            };
            Classes[CLASS_FIGHTER].LevelRequirements = new Dictionary<int, int> {
                { 1, 0 },
                { 2, 2035 },
                { 3, 4065 },
                { 4, 8125 },
                { 5, 16251 },
                { 6, 32501 },
                { 7, 65001 },
                { 8, 120001 },
                { 9, 240001 },
                { 10, 360001 },
                { 11, 480001 },
                { 12, 600001 },
                { 13, 720001 },
                { 14, 840001 },
                { 15, 960001 },
                { 16, 1080001 },
                { 17, 1200001 },
                { 18, 1320001 },
                { 19, 1440001 },
                { 20, 1560001 },
            };
            Classes[CLASS_ILLUSIONIST].LevelRequirements = new Dictionary<int, int> {
                { 1, 0 },
                { 2, 2251 },
                { 3, 4501 },
                { 4, 9001 },
                { 5, 18001 },
                { 6, 36001 },
                { 7, 80001 },
                { 8, 160001 },
                { 9, 310001 },
                { 10, 450001 },
                { 11, 600001 },
                { 12, 750001 },
                { 13, 850001 },
                { 14, 950001 },
                { 15, 1050001 },
                { 16, 1150001 },
                { 17, 1250001 },
                { 18, 1350001 },
                { 19, 1450001 },
                { 20, 1550001 },
            };
            Classes[CLASS_MAGIC_USER].LevelRequirements = new Dictionary<int, int> {
                { 1, 0 },
                { 2, 2501 },
                { 3, 5001 },
                { 4, 10001 },
                { 5, 20001 },
                { 6, 40001 },
                { 7, 80001 },
                { 8, 160001 },
                { 9, 310001 },
                { 10, 460001 },
                { 11, 610001 },
                { 12, 760001 },
                { 13, 910001 },
                { 14, 1060001 },
                { 15, 1210001 },
                { 16, 1360001 },
                { 17, 1510001 },
                { 18, 1660001 },
                { 19, 1810001 },
                { 20, 1960001 },
            };
            Classes[CLASS_MONK].LevelRequirements = new Dictionary<int, int> {
                { 1, 0 },
                { 2, 2235 },
                { 3, 4765 },
                { 4, 10025 },
                { 5, 18251 },
                { 6, 45501 },
                { 7, 93001 },
                { 8, 195001 },
                { 9, 340001 },
                { 10, 560001 },
                { 11, 780001 },
                { 12, 1000001 },
                { 13, 1220001 },
                { 14, 1440001 },
                { 15, 1660001 },
                { 16, 1880001 }
            };
            Classes[CLASS_PALADIN].LevelRequirements = new Dictionary<int, int> {
                { 1, 0 },
                { 2, 2735 },
                { 3, 5465 },
                { 4, 11025 },
                { 5, 20251 },
                { 6, 42501 },
                { 7, 90001 },
                { 8, 170001 },
                { 9, 340001 },
                { 10, 560001 },
                { 11, 780001 },
                { 12, 1000001 },
                { 13, 1220001 },
                { 14, 1440001 },
                { 15, 1660001 },
                { 16, 1880001 },
                { 17, 2100001 },
                { 18, 2320001 },
                { 19, 2540001 },
                { 20, 2780001 },
            };
            Classes[CLASS_RANGER].LevelRequirements = new Dictionary<int, int> {
                { 1, 0 },
                { 2, 2235 },
                { 3, 4465 },
                { 4, 8925 },
                { 5, 17851 },
                { 6, 35701 },
                { 7, 71401 },
                { 8, 135001 },
                { 9, 255001 },
                { 10, 375001 },
                { 11, 495001 },
                { 12, 615001 },
                { 13, 735001 },
                { 14, 855001 },
                { 15, 975001 },
                { 16, 1095001 },
                { 17, 1215001 },
                { 18, 1335001 },
                { 19, 1455001 },
                { 20, 1575001 },
            };
            Classes[CLASS_THIEF].LevelRequirements = new Dictionary<int, int> {
                { 1, 0 },
                { 2, 1251 },
                { 3, 2501 },
                { 4, 5001 },
                { 5, 10001 },
                { 6, 20001 },
                { 7, 40001 },
                { 8, 80001 },
                { 9, 160001 },
                { 10, 280001 },
                { 11, 400001 },
                { 12, 520001 },
                { 13, 640001 },
                { 14, 760001 },
                { 15, 880001 },
                { 16, 1000001 },
                { 17, 1120001 },
                { 18, 1240001 },
                { 19, 1360001 },
                { 20, 1480001 },
            };
            #endregion
            #region RACIAL RESTRICTIONS
            Classes[CLASS_ASSASSIN].RacialRestrictions = new Dictionary<int, bool> {
                { LabLordRace.RACE_DWARF, true },
                { LabLordRace.RACE_ELF, true },
                { LabLordRace.RACE_GNOME, true },
                { LabLordRace.RACE_HALFLING, false },
                { LabLordRace.RACE_HALF_ELF, true },
                { LabLordRace.RACE_HALF_ORC, true },
                { LabLordRace.RACE_HUMAN, true }
            };
            Classes[CLASS_CLERIC].RacialRestrictions = new Dictionary<int, bool> {
                { LabLordRace.RACE_DWARF, true },
                { LabLordRace.RACE_ELF, true },
                { LabLordRace.RACE_GNOME, true },
                { LabLordRace.RACE_HALFLING, false },
                { LabLordRace.RACE_HALF_ELF, true },
                { LabLordRace.RACE_HALF_ORC, true },
                { LabLordRace.RACE_HUMAN, true }
            };
            Classes[CLASS_DRUID].RacialRestrictions = new Dictionary<int, bool> {
                { LabLordRace.RACE_DWARF, false },
                { LabLordRace.RACE_ELF, false },
                { LabLordRace.RACE_GNOME, false },
                { LabLordRace.RACE_HALFLING, false },
                { LabLordRace.RACE_HALF_ELF, false },
                { LabLordRace.RACE_HALF_ORC, false },
                { LabLordRace.RACE_HUMAN, true }
            };
            Classes[CLASS_FIGHTER].RacialRestrictions = new Dictionary<int, bool> {
                { LabLordRace.RACE_DWARF, true },
                { LabLordRace.RACE_ELF, true },
                { LabLordRace.RACE_GNOME, true },
                { LabLordRace.RACE_HALFLING, true },
                { LabLordRace.RACE_HALF_ELF, true },
                { LabLordRace.RACE_HALF_ORC, true },
                { LabLordRace.RACE_HUMAN, true }
            };
            Classes[CLASS_ILLUSIONIST].RacialRestrictions = new Dictionary<int, bool> {
                { LabLordRace.RACE_DWARF, false },
                { LabLordRace.RACE_ELF, false },
                { LabLordRace.RACE_GNOME, true },
                { LabLordRace.RACE_HALFLING, false },
                { LabLordRace.RACE_HALF_ELF, false },
                { LabLordRace.RACE_HALF_ORC, false },
                { LabLordRace.RACE_HUMAN, true }
            };
            Classes[CLASS_MAGIC_USER].RacialRestrictions = new Dictionary<int, bool> {
                { LabLordRace.RACE_DWARF, false },
                { LabLordRace.RACE_ELF, true },
                { LabLordRace.RACE_GNOME, false },
                { LabLordRace.RACE_HALFLING, false },
                { LabLordRace.RACE_HALF_ELF, true },
                { LabLordRace.RACE_HALF_ORC, false },
                { LabLordRace.RACE_HUMAN, true }
            };
            Classes[CLASS_MONK].RacialRestrictions = new Dictionary<int, bool> {
                { LabLordRace.RACE_DWARF, false },
                { LabLordRace.RACE_ELF, false },
                { LabLordRace.RACE_GNOME, false },
                { LabLordRace.RACE_HALFLING, false },
                { LabLordRace.RACE_HALF_ELF, false },
                { LabLordRace.RACE_HALF_ORC, false },
                { LabLordRace.RACE_HUMAN, true }
            };
            Classes[CLASS_PALADIN].RacialRestrictions = new Dictionary<int, bool> {
                { LabLordRace.RACE_DWARF, false },
                { LabLordRace.RACE_ELF, false },
                { LabLordRace.RACE_GNOME, false },
                { LabLordRace.RACE_HALFLING, false },
                { LabLordRace.RACE_HALF_ELF, false },
                { LabLordRace.RACE_HALF_ORC, false },
                { LabLordRace.RACE_HUMAN, true }
            };
            Classes[CLASS_RANGER].RacialRestrictions = new Dictionary<int, bool> {
                { LabLordRace.RACE_DWARF, false },
                { LabLordRace.RACE_ELF, false },
                { LabLordRace.RACE_GNOME, false },
                { LabLordRace.RACE_HALFLING, false },
                { LabLordRace.RACE_HALF_ELF, true },
                { LabLordRace.RACE_HALF_ORC, false },
                { LabLordRace.RACE_HUMAN, true }
            };
            Classes[CLASS_THIEF].RacialRestrictions = new Dictionary<int, bool> {
                { LabLordRace.RACE_DWARF, true },
                { LabLordRace.RACE_ELF, true },
                { LabLordRace.RACE_GNOME, true },
                { LabLordRace.RACE_HALFLING, true },
                { LabLordRace.RACE_HALF_ELF, true },
                { LabLordRace.RACE_HALF_ORC, true },
                { LabLordRace.RACE_HUMAN, true }
            };
            #endregion
            #region THAC0
            Dictionary<List<int>, int> CLERIC_THIEF_THAC0 = new Dictionary<List<int>, int>
            {
                { new List<int>() { 1, 2, 3 }, 19 },
                { new List<int>() { 4, 5 },    18 },
                { new List<int>() { 6, 7, 8 }, 17 },
                { new List<int>() { 9, 10 },   16 },
                { new List<int>() { 11 },      15 },
                { new List<int>() { 12 },      14 },
                { new List<int>() { 13, 14 },  13 },
                { new List<int>() { 15, 16 },  12 },
                { new List<int>() { 17, 18 },  11 },
                { new List<int>() { 19, 20 },  10 },
                { new List<int>() { 21 },       9 }
            };
            Dictionary<List<int>, int> FIGHTER_THAC0 = new Dictionary<List<int>, int>
            {
                { new List<int>() { 1, 2 },   19 },
                { new List<int>() { 3 },      18 },
                { new List<int>() { 4 },      17 },
                { new List<int>() { 5 },      16 },
                { new List<int>() { 6 },      15 },
                { new List<int>() { 7, 8 },   14 },
                { new List<int>() { 9 },      13 },
                { new List<int>() { 10, 11 }, 12 },
                { new List<int>() { 12 },     11 },
                { new List<int>() { 13 },     10 },
                { new List<int>() { 14 },      9 },
                { new List<int>() { 15 },      8 },
                { new List<int>() { 16 },      7 },
                { new List<int>() { 18 },      6 },
                { new List<int>() { 18 },      5 },
                { new List<int>() { 19 },      4 }
            };
            Dictionary<List<int>, int> MAGE_THAC0 = new Dictionary<List<int>, int>
            {
                { new List<int>() { 1, 2, 3 },    19 },
                { new List<int>() { 4, 5, 6, 7 }, 18 },
                { new List<int>() { 8, 9, 10 },   17 },
                { new List<int>() { 11, 12 },     16 },
                { new List<int>() { 13 },         15 },
                { new List<int>() { 14, 15 },     14 },
                { new List<int>() { 16, 17, 18 }, 13 },
                { new List<int>() { 19, 20 },     12 },
                { new List<int>() { 21, 22, 23 }, 11 },
                { new List<int>() { 24 },         10 }
            };
            Classes[CLASS_ASSASSIN].thac0 = CLERIC_THIEF_THAC0;
            Classes[CLASS_CLERIC].thac0 = CLERIC_THIEF_THAC0;
            Classes[CLASS_DRUID].thac0 = CLERIC_THIEF_THAC0;
            Classes[CLASS_MONK].thac0 = CLERIC_THIEF_THAC0;
            Classes[CLASS_THIEF].thac0 = CLERIC_THIEF_THAC0;

            Classes[CLASS_FIGHTER].thac0 = FIGHTER_THAC0;
            Classes[CLASS_PALADIN].thac0 = FIGHTER_THAC0;
            Classes[CLASS_RANGER].thac0 = FIGHTER_THAC0;

            Classes[CLASS_ILLUSIONIST].thac0 = MAGE_THAC0;
            Classes[CLASS_MAGIC_USER].thac0 = MAGE_THAC0;
            #endregion
            #region SAVING THROWS
            Dictionary<List<int>, Dictionary<int, int>> CLERIC_DRUID_MONK_SAVING_THROWS = new Dictionary<List<int>, Dictionary<int, int>>
            {
                {
                    new List<int>() { 1, 2, 3, 4 },
                    new Dictionary<int, int>
                    {
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH,   16 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON,   11 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_DEATH,    11 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PETRIFY,  14 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PARALYZE, 14 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,    12 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,   15 }
                    }
                },
                {
                    new List<int>() { 5, 6, 7, 8 },
                    new Dictionary<int, int>
                    {
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH,   14 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON,    9 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_DEATH,     9 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PETRIFY,  12 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PARALYZE, 12 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,    10 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,   12 }
                    }
                },
                {
                    new List<int>() { 9, 10, 11, 12 },
                    new Dictionary<int, int>
                    {
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH,   12 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON,    7 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_DEATH,     7 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PETRIFY,  10 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PARALYZE, 10 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,     8 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,    9 }
                    }
                },
                {
                    new List<int>() { 13, 14, 15, 16 },
                    new Dictionary<int, int>
                    {
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH,    8 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON,    3 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_DEATH,     3 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PETRIFY,   8 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PARALYZE,  8 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,     4 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,    6 }
                    }
                },
                {
                    new List<int>() { 17 },
                    new Dictionary<int, int>
                    {
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH,    6 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON,    2 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_DEATH,     2 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PETRIFY,   6 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PARALYZE,  6 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,     4 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,    5 }
                    }
                }
            };
            Dictionary<List<int>, Dictionary<int, int>> FIGHTER_SAVING_THROWS = new Dictionary<List<int>, Dictionary<int, int>>
            {
                {
                    new List<int>() { 0 },
                    new Dictionary<int, int>
                    {
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH,   17 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON,   14 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_DEATH,    14 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PETRIFY,  16 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PARALYZE, 16 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,    15 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,   18 }
                    }
                },
                {
                    new List<int>() { 1, 2, 3 },
                    new Dictionary<int, int>
                    {
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH,   15 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON,   12 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_DEATH,    12 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PETRIFY,  14 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PARALYZE, 14 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,    13 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,   16 }
                    }
                },
                {
                    new List<int>() { 4, 5, 6 },
                    new Dictionary<int, int>
                    {
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH,   13 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON,   10 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_DEATH,    10 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PETRIFY,  12 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PARALYZE, 12 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,    11 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,   14 }
                    }
                },
                {
                    new List<int>() { 7, 8, 9 },
                    new Dictionary<int, int>
                    {
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH,    9 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON,    8 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_DEATH,     8 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PETRIFY,  10 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PARALYZE, 10 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,     9 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,   12 }
                    }
                },
                {
                    new List<int>() { 10, 11, 12 },
                    new Dictionary<int, int>
                    {
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH,    7 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON,    6 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_DEATH,     6 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PETRIFY,   8 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PARALYZE,  8 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,     7 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,   10 }
                    }
                },
                {
                    new List<int>() { 13, 14, 15 },
                    new Dictionary<int, int>
                    {
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH,    5 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON,    4 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_DEATH,     4 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PETRIFY,   6 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PARALYZE,  6 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,     5 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,    8 }
                    }
                },
                {
                    new List<int>() { 16, 17, 18 },
                    new Dictionary<int, int>
                    {
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH,    4 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON,    4 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_DEATH,     4 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PETRIFY,   5 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PARALYZE,  5 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,     4 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,    7 }
                    }
                },
                {
                    new List<int>() { 19 },
                    new Dictionary<int, int>
                    {
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH,    4 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON,    3 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_DEATH,     3 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PETRIFY,   4 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PARALYZE,  4 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,     3 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,    6 }
                    }
                },
            };
            Dictionary<List<int>, Dictionary<int, int>> MAGE_SAVING_THROWS = new Dictionary<List<int>, Dictionary<int, int>>
            {
                {
                    new List<int>() { 1, 2, 3, 4, 5 },
                    new Dictionary<int, int>
                    {
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH,   16 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON,   13 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_DEATH,    13 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PETRIFY,  13 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PARALYZE, 13 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,    13 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,   14 }
                    }
                },
                {
                    new List<int>() { 6, 7, 8, 9, 10 },
                    new Dictionary<int, int>
                    {
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH,   14 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON,   11 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_DEATH,    11 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PETRIFY,  11 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PARALYZE, 11 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,    11 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,   12 }
                    }
                },
                {
                    new List<int>() { 11, 12, 13, 14, 15 },
                    new Dictionary<int, int>
                    {
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH,   12 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON,    9 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_DEATH,     9 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PETRIFY,   9 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PARALYZE,  9 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,     9 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,    8 }
                    }
                },
                {
                    new List<int>() { 16, 17, 18 },
                    new Dictionary<int, int>
                    {
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH,    8 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON,    7 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_DEATH,     7 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PETRIFY,   6 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PARALYZE,  6 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,     5 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,    6 }
                    }
                },
                {
                    new List<int>() { 19 },
                    new Dictionary<int, int>
                    {
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH,    7 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON,    6 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_DEATH,     6 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PETRIFY,   5 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PARALYZE,  5 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,     4 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,    4 }
                    }
                }
            };
            Dictionary<List<int>, Dictionary<int, int>> THIEF_SAVING_THROWS = new Dictionary<List<int>, Dictionary<int, int>>
            {
                {
                    new List<int>() { 1, 2, 3, 4 },
                    new Dictionary<int, int>
                    {
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH,   16 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON,   14 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_DEATH,    14 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PETRIFY,  13 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PARALYZE, 13 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,    15 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,   14 }
                    }
                },
                {
                    new List<int>() { 5, 6, 7, 8 },
                    new Dictionary<int, int>
                    {
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH,   14 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON,   12 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_DEATH,    12 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PETRIFY,  11 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PARALYZE, 11 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,    13 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,   12 }
                    }
                },
                {
                    new List<int>() { 9, 10, 11, 12 },
                    new Dictionary<int, int>
                    {
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH,   12 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON,   10 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_DEATH,    10 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PETRIFY,   9 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PARALYZE,  9 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,    11 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,   10 }
                    }
                },
                {
                    new List<int>() { 13, 14, 15, 16 },
                    new Dictionary<int, int>
                    {
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH,   10 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON,    8 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_DEATH,     8 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PETRIFY,   7 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PARALYZE,  7 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,     9 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,    8 }
                    }
                },
                {
                    new List<int>() { 17 },
                    new Dictionary<int, int>
                    {
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH,    8 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON,    6 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_DEATH,     6 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PETRIFY,   5 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PARALYZE,  5 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,     7 },
                        { LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,    6 }
                    }
                }
            };
            Classes[CLASS_ASSASSIN].savingThrows = THIEF_SAVING_THROWS;
            Classes[CLASS_THIEF].savingThrows = THIEF_SAVING_THROWS;

            Classes[CLASS_CLERIC].savingThrows = CLERIC_DRUID_MONK_SAVING_THROWS;
            Classes[CLASS_DRUID].savingThrows = CLERIC_DRUID_MONK_SAVING_THROWS;
            Classes[CLASS_MONK].savingThrows = CLERIC_DRUID_MONK_SAVING_THROWS;

            Classes[CLASS_FIGHTER].savingThrows = FIGHTER_SAVING_THROWS;
            Classes[CLASS_PALADIN].savingThrows = FIGHTER_SAVING_THROWS;
            Classes[CLASS_RANGER].savingThrows = FIGHTER_SAVING_THROWS;

            Classes[CLASS_ILLUSIONIST].savingThrows = MAGE_SAVING_THROWS;
            Classes[CLASS_MAGIC_USER].savingThrows = MAGE_SAVING_THROWS;
            #endregion
        }
        #endregion
        public static string[] LEVEL_TITLE = {
        "1st level",
        "2nd level",
        "3rd level",
        "4th level",
        "5th level",
        "6th level",
        "7th level",
        "8th level",
        "9th level",
        "10th level",
        };
        /// <summary>
        /// Flag indicating a class hasn't been assigned.
        /// </summary>
        public static int CLASS_UNASSIGNED = -1;
        /// <summary>
        /// The list of modifiers applied for the specific profession.
        /// </summary>
        public EquipmentItemModifier[] Modifiers = new EquipmentItemModifier[LabLordGlobals.NUM_ELEMENTS];
        private string StandardTooltip(int race)
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append(Description);
            sb.Append("\n\n");
            if (PrimeRequisites.Length > 0)
            {
                sb.Append("Prime Requisite:\t");
                for (int i = 0, li = PrimeRequisites.Length; i < li; i++)
                {
                    sb.Append(LabLordAbility.Abilities[PrimeRequisites[i]].Abbr);
                    if (i + 1 < li)
                    {
                        sb.Append(", ");
                    }
                }
                sb.Append("\n");
            }
            string s = BuildClassDescription(sb.ToString(), HitDice, MaxLevelForRace[race], LevelRequirements[2]);
            sb.ReturnToPool();
            return s;
        }
        private string ClassUnavailableTooltip(int race, int[] abilities)
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            PooledStringBuilder sb2 = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append(Title);
            sb.Append(" Unavailable\n\n");
            if (!RacialRestrictions[race])
            {
                sb.Append("<b>");
                sb.Append(LabLordRace.Races[race].Plural);
                sb.Append("</b> Not Allowed");
            }
            else
            {
                List<string> list = new List<string>();
                for (int ability = 0, li = abilities.Length; ability < li; ability++)
                {
                    if (!requirements.ContainsKey(ability))
                    {
                        continue;
                    }
                    int score = abilities[ability];
                    int min = requirements[ability];
                    if (score < min)
                    {
                        sb2.Append("<b><color=red>");
                        sb2.Append(LabLordAbility.Abilities[ability].Abbr);
                        sb2.Append("</color></b> ");
                        sb2.Append(min);
                        sb2.Append(" or more");
                        list.Add(sb2.ToString());
                        sb2.Length = 0;
                    }
                    else
                    {
                        sb2.Append("<b>");
                        sb2.Append(LabLordAbility.Abilities[ability].Abbr);
                        sb2.Append("</b> ");
                        sb2.Append(min);
                        sb2.Append(" or more");
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
                    sb.Append("Cuckoo Bananas! My BRaIn must be BU5+3D!!");
                }
            }
            string s = sb.ToString();
            sb.ReturnToPool();
            sb2.ReturnToPool();
            return s;
        }
        public string GetDescription(int[] scores, int race, bool available)
        {
            if (debug)
            {
                UnityEngine.Debug.Log(Title + " available? " + available);
            }
            string s = "";
            if (available)
            {
                s = StandardTooltip(race);
            }
            else
            {
                s = ClassUnavailableTooltip(race, scores);
            }
            return s;
        }
        public int RollHpForNewLevel()
        {
            // TODO - implement check for level 9 or above
            return Diceroller.Instance.RolldX(HitDice);
        }
        public int GetXpNeededForNextLevel(int level)
        {
            return LevelRequirements[level];
        }
        private string BuildClassDescription(string s, int hd, int maxLevel, int xp)
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append(s);
            sb.Append("Hit Dice:\t\t\t\t1D");
            sb.Append(hd);
            sb.Append("\nMaximum Level:\t");
            if (maxLevel < 999)
            {
                sb.Append(maxLevel);
            }
            else
            {
                sb.Append("Unlimited");
            }
            sb.Append("\nXP for Next Level:\t");
            sb.Append(xp);
            string outs = sb.ToString();
            sb.ReturnToPool();
            return outs;
        }
        /// <summary>
        /// The field defining the list of ability requirements.
        /// </summary>
        private Dictionary<int, int> requirements;
        /// <summary>
        /// The property defining the list of ability requirements.
        /// </summary>
        public Dictionary<int, int> Requirements { get { return requirements; } }
        public int[] PrimeRequisites;
        public Dictionary<int, int> MaxLevelForRace;
        public Dictionary<int, int> LevelRequirements;
        private Dictionary<int, bool> RacialRestrictions;
        public int HitDice;
        /// <summary>
        /// The field defining the list of ability modifiers.  First lookup is the value range, then the list of modifiers.
        /// </summary>
        private Dictionary<List<int>, int> thac0;
        /// <summary>
        /// The field defining the list of ability modifiers.  First lookup is the value range, then the list of modifiers.
        /// </summary>
        private Dictionary<List<int>, Dictionary<int, int>> savingThrows;
        /// <summary>
        /// Hidden constructor.
        /// </summary>
        private LabLordClass(int c, string t, string d, Dictionary<int, int> dict, int hd) : base(c, t, d)
        {
            requirements = dict;
            PrimeRequisites = new int[0];
            HitDice = hd;
        }
        /// <summary>
        /// Gets the THAC0 score for the class's level.
        /// </summary>
        /// <param name="level">the level</param>
        /// <returns><see cref="Int32"/></returns>
        public int GetTHAC0ForLevel(int level)
        {
            int score = 0, iter = 0;
            foreach (KeyValuePair<List<int>, int> entry in thac0)
            {
                iter++;
                // do something with entry.Value or entry.Key
                if (entry.Key.Contains(level))
                {
                    score = entry.Value;
                    break;
                }
                else if (thac0[entry.Key] == thac0.Last().Value)
                {
                    // last entry in the list. if level hasn't been found,
                    // it must have been higher than last entry
                    score = entry.Value;
                    break;
                }
            }
            return score;
        }
        public int GetSavingThrowForLevel(int saveType, int level)
        {
            int score = 0;
            foreach (KeyValuePair<List<int>, Dictionary<int, int>> entry in savingThrows)
            {
                // do something with entry.Value or entry.Key
                if (entry.Key.Contains(level))
                {
                    score = entry.Value[saveType];
                    break;
                }
                else if (savingThrows[entry.Key] == savingThrows.Last().Value)
                {
                    // last entry in the list. if level hasn't been found,
                    // it must have been higher than last entry
                    score = entry.Value[saveType];
                    break;
                }
            }
            return score;
        }
    }
}
