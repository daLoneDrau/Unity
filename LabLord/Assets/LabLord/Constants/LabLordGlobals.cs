namespace LabLord.Constants
{
    public sealed class LabLordGlobals
    {
        //****************************
        // EQUIPMENT SLOTS
        //****************************
        /// <summary>
        /// weapon slot.
        /// </summary>
        public const int EQUIP_SLOT_WEAPON = 2;
        /// <summary>
        /// shield slot.
        /// </summary>
        public const int EQUIP_SLOT_SHIELD = 1;
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
        /// <summary>
        /// Dexterity.
        /// </summary>
        public const int EQUIP_ELEMENT_DEX = 0;
        /// <summary>
        /// Charisma.
        /// </summary>
        public const int EQUIP_ELEMENT_CHA = 1;
        /// <summary>
        /// Constitution.
        /// </summary>
        public const int EQUIP_ELEMENT_CON = 2;
        /// <summary>
        /// Hit Points.
        /// </summary>
        public const int EQUIP_ELEMENT_HP = 3;
        /// <summary>
        /// Intelligence.
        /// </summary>
        public const int EQUIP_ELEMENT_INT = 4;
        /// <summary>
        /// Max Hit Points.
        /// </summary>
        public const int EQUIP_ELEMENT_MHP = 5;
        /// <summary>
        /// Save vs Breath Weapon
        /// </summary>
        public const int EQUIP_ELEMENT_SBW = 6;
        /// <summary>
        /// Save vs Gaze
        /// </summary>
        public const int EQUIP_ELEMENT_SGZ = 7;
        /// <summary>
        /// Save vs Magic Wand
        /// </summary>
        public const int EQUIP_ELEMENT_SMW = 8;
        /// <summary>
        /// Save vs Ray or Poison
        /// </summary>
        public const int EQUIP_ELEMENT_SRP = 9;
        /// <summary>
        /// Save vs Staff or Spell
        /// </summary>
        public const int EQUIP_ELEMENT_SSS = 10;
        /// <summary>
        /// Strength.
        /// </summary>
        public const int EQUIP_ELEMENT_STR = 11;
        /// <summary>
        /// Wisdom.
        /// </summary>
        public const int EQUIP_ELEMENT_WIS = 12;
        /// <summary>
        /// Armour Class.
        /// </summary>
        public const int EQUIP_ELEMENT_AC = 13;
        /// <summary>
        /// To Hit Modifier.
        /// </summary>
        public const int EQUIP_ELEMENT_THM = 14;
        public const int NUM_ELEMENTS = 15;
        public const int SM_300_CHAR_WIZARD_STEP_ONE = 300;
        /// <summary>
        /// the Dwarf race.
        /// </summary>
        public const int RACE_DWARF = 1;
        /// <summary>
        /// the Elf race.
        /// </summary>
        public const int RACE_ELF = 2;
        /// <summary>
        /// the Gnome race.
        /// </summary>
        public const int RACE_GNOME = 4;
        /// <summary>
        /// the Halfling race.
        /// </summary>
        public const int RACE_HALFLING = 8;
        /// <summary>
        /// the Half-Elf race.
        /// </summary>
        public const int RACE_HALF_ELF = 16;
        /// <summary>
        /// the Half-Orc race.
        /// </summary>
        public const int RACE_HALF_ORC = 32;
        /// <summary>
        /// the Human race.
        /// </summary>
        public const int RACE_HUMAN = 64;
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