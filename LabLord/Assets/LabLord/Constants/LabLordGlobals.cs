namespace LabLord.Constants
{
    public sealed class LabLordGlobals
    {
        //****************************
        // EQUIPMENT SLOTS
        //****************************
        /// <summary>
        /// the number of equipment slots
        /// <para>0 - Left hand ring</para>
        /// <para>1 - Right hand ring</para>
        /// <para>2 - weapon</para>
        /// <para>3 - shield</para>
        /// <para>4 - torch</para>
        /// <para>5 - torso</para>
        /// <para>6 - helmet</para>
        /// <para>7 - leggings</para>
        /// see complete list in <see cref="EquipmentGlobals"/>
        /// </summary>
        public const int MAX_EQUIPPED = 8;
        //****************************
        // EQUIPMENT ELEMENTS
        //****************************
        #region ABILITIES - 6 items
        /// <summary>
        /// Strength.
        /// </summary>
        public const int EQUIP_ELEMENT_STR = 0;
        /// <summary>
        /// Dexterity.
        /// </summary>
        public const int EQUIP_ELEMENT_DEX = 1;
        /// <summary>
        /// Constitution.
        /// </summary>
        public const int EQUIP_ELEMENT_CON = 2;
        /// <summary>
        /// Intelligence.
        /// </summary>
        public const int EQUIP_ELEMENT_INT = 3;
        /// <summary>
        /// Wisdom.
        /// </summary>
        public const int EQUIP_ELEMENT_WIS = 4;
        /// <summary>
        /// Charisma.
        /// </summary>
        public const int EQUIP_ELEMENT_CHA = 5;
        #endregion
        #region SAVING THROWS - 7 items
        /// <summary>
        /// Save vs Breath Weapon
        /// </summary>
        public const int EQUIP_ELEMENT_SAVE_V_BREATH = 6;
        /// <summary>
        /// Save vs Poison
        /// </summary>
        public const int EQUIP_ELEMENT_SAVE_V_POISON = 7;
        /// <summary>
        /// Save vs Death
        /// </summary>
        public const int EQUIP_ELEMENT_SAVE_V_DEATH = 8;
        /// <summary>
        /// Save vs Petrify
        /// </summary>
        public const int EQUIP_ELEMENT_SAVE_V_PETRIFY = 9;
        /// <summary>
        /// Save vs PARALYZE
        /// </summary>
        public const int EQUIP_ELEMENT_SAVE_V_PARALYZE = 10;
        /// <summary>
        /// Save vs WANDS
        /// </summary>
        public const int EQUIP_ELEMENT_SAVE_V_WANDS = 11;
        /// <summary>
        /// Save vs SPELLS
        /// </summary>
        public const int EQUIP_ELEMENT_SAVE_V_SPELLS = 12;
        #endregion
        #region THIEF SKILLS - 7 items
        /// <summary>
        /// THIEF PICK LOCKS
        /// </summary>
        public const int EQUIP_ELEMENT_THIEF_PICK_LOCKS = 13;
        /// <summary>
        /// THIEF FIND AND REMOVE TRAPS
        /// </summary>
        public const int EQUIP_ELEMENT_THIEF_FIND_REMOVE_TRAPS = 14;
        /// <summary>
        /// THIEF PICK POCKETS
        /// </summary>
        public const int EQUIP_ELEMENT_THIEF_PICK_POCKETS = 15;
        /// <summary>
        /// THIEF MOVE SILENTLY
        /// </summary>
        public const int EQUIP_ELEMENT_THIEF_MOVE_SILENTLY = 16;
        /// <summary>
        /// THIEF CLIMB WALLS
        /// </summary>
        public const int EQUIP_ELEMENT_THIEF_CLIMB_WALLS = 17;
        /// <summary>
        /// THIEF HIDE IN SHADOWS
        /// </summary>
        public const int EQUIP_ELEMENT_THIEF_HIDE_SHADOWS = 18;
        /// <summary>
        /// THIEF HEAR NOISE
        /// </summary>
        public const int EQUIP_ELEMENT_THIEF_HEAR_NOISE = 19;
        #endregion
        #region STRENGTH MODIFIERS - 3 items
        /// <summary>
        /// To Hit Modifier.
        /// </summary>
        public const int EQUIP_ELEMENT_THM = 20;
        /// <summary>
        /// Damage Modifier.
        /// </summary>
        public const int EQUIP_ELEMENT_DMG = 21;
        /// <summary>
        /// Force Doors Modifier.
        /// </summary>
        public const int EQUIP_ELEMENT_FORCE_DOORS = 22;
        #endregion
        #region DEXTERITY MODIFIERS
        /// <summary>
        /// Armour Class.
        /// </summary>
        public const int EQUIP_ELEMENT_AC = 23;
        /// <summary>
        /// Missile To Hit Modifier.
        /// </summary>
        public const int EQUIP_ELEMENT_MISSILE_THM = 24;
        /// <summary>
        /// Initiative Modifier.
        /// </summary>
        public const int EQUIP_ELEMENT_INITIATIVE = 25;
        #endregion
        #region CONSITUTION MODIFIERS
        /// <summary>
        /// Hit Points.
        /// </summary>
        public const int EQUIP_ELEMENT_HP = 26;
        /// <summary>
        /// Max Hit Points.
        /// </summary>
        public const int EQUIP_ELEMENT_MHP = 27;
        /// <summary>
        /// Survive Resurrection.
        /// </summary>
        public const int EQUIP_SURVIVE_RESURRECTION = 28;
        /// <summary>
        /// Survive Polymorph.
        /// </summary>
        public const int EQUIP_SURVIVE_POLYMORPH = 29;
        #endregion
        #region INTELLIGENCE MODIFIERS
        /// <summary>
        /// Bonus Languages.
        /// </summary>
        public const int EQUIP_ELEMENT_ADDL_LANGUAGES = 30;
        /// <summary>
        /// Language Proficiency.
        /// </summary>
        public const int EQUIP_ELEMENT_LANGUAGE_PROFICIENCY = 31;
        /// <summary>
        /// % Chance to Learn Spell.
        /// </summary>
        public const int EQUIP_ELEMENT_LEARN_SPELL = 32;
        /// <summary>
        /// Minimum Spells per Level.
        /// </summary>
        public const int EQUIP_ELEMENT_MIN_SPELLS = 33;
        /// <summary>
        /// Maximum Spells per Level.
        /// </summary>
        public const int EQUIP_ELEMENT_MAX_SPELLS = 34;
        #endregion
        #region WISDOM MODIFIERS
        /// <summary>
        /// % Chance of Divine Spell Failure.
        /// </summary>
        public const int EQUIP_ELEMENT_DIVINE_SPELL_FAILURE = 35;
        /// <summary>
        /// % Chance of Divine Spell Failure.
        /// </summary>
        public const int EQUIP_ELEMENT_BONUS_LVL_1_SPELLS = 36;
        /// <summary>
        /// % Chance of Divine Spell Failure.
        /// </summary>
        public const int EQUIP_ELEMENT_BONUS_LVL_2_SPELLS = 37;
        /// <summary>
        /// % Chance of Divine Spell Failure.
        /// </summary>
        public const int EQUIP_ELEMENT_BONUS_LVL_3_SPELLS = 38;
        /// <summary>
        /// % Chance of Divine Spell Failure.
        /// </summary>
        public const int EQUIP_ELEMENT_BONUS_LVL_4_SPELLS = 39;
        #endregion
        #region CHARISMA MODIFIERS
        /// <summary>
        /// Reaction Adjustment.
        /// </summary>
        public const int EQUIP_ELEMENT_REACTION_ADJUSTMENT = 40;
        /// <summary>
        /// # of Hirelings.
        /// </summary>
        public const int EQUIP_ELEMENT_NUM_HIRELIGNS = 41;
        /// <summary>
        /// Retainer Morale.
        /// </summary>
        public const int EQUIP_ELEMENT_RETAINER_MORALE = 42;
        #endregion
        public const int NUM_ELEMENTS = 43;
        public const int MODIFIER_SRC_RACE = 0;
        public const int MODIFIER_SRC_ABILITY = 1;
        public const int SM_300_CHAR_WIZARD_STEP_ONE = 300;
        /// <summary>
        /// the Assassin class.
        /// </summary>
        public const int CLASS_ASSASSIN = 1;
        /// <summary>
        /// the Cleric class.
        /// </summary>
        public const int CLASS_CLERIC = 2;
        /// <summary>
        /// the Druid class.
        /// </summary>
        public const int CLASS_DRUID = 4;
        /// <summary>
        /// the Fighter class.
        /// </summary>
        public const int CLASS_FIGHTER = 8;
        /// <summary>
        /// the Illusionist class.
        /// </summary>
        public const int CLASS_ILLUSIONIST = 16;
        /// <summary>
        /// the Magic-User class.
        /// </summary>
        public const int CLASS_MAGIC_USER = 32;
        /// <summary>
        /// the Monk class.
        /// </summary>
        public const int CLASS_MONK = 64;
        /// <summary>
        /// the Paladin class.
        /// </summary>
        public const int CLASS_PALADIN = 128;
        /// <summary>
        /// the Ranger class.
        /// </summary>
        public const int CLASS_RANGER = 256;
        /// <summary>
        /// the Thief class.
        /// </summary>
        public const int CLASS_THIEF = 512;
    }
}