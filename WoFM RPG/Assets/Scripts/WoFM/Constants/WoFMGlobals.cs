using UnityEngine;

namespace WoFM.Constants
{
    public sealed class WoFMGlobals
    {
        //****************************
        // DIRECTIONS
        //****************************
        /// <summary>
        /// North
        /// </summary>
        public const int NORTH = 0;
        /// <summary>
        /// East
        /// </summary>
        public const int EAST = 1;
        /// <summary>
        /// South
        /// </summary>
        public const int SOUTH = 2;
        /// <summary>
        /// West
        /// </summary>
        public const int WEST = 3;
        /// <summary>
        /// the list of directions.
        /// </summary>
        public static Vector2[] DIRECTIONS = new Vector2[] {
            new Vector2(0, 1), // NORTH
            new Vector2(1, 0), // EAST
            new Vector2(0, -1), // SOUTH
            new Vector2(-1, 0), // WEST
            };
        //****************************
        // EQUIPMENT SLOTS
        //****************************
        /// <summary>
        /// weapon slot.
        /// </summary>
        public const int EQUIP_SLOT_WEAPON = 0;
        /// <summary>
        /// shield slot.
        /// </summary>
        public const int SHIELD_SLOT_WEAPON = 1;
        /// <summary>
        /// The # of equipment slots.
        /// </summary>
        public const int MAX_EQUIPPED = 2;
        //****************************
        // EQUIPMENT ELEMENTS
        //****************************
        /// <summary>
        /// Stamina.
        /// </summary>
        public const int EQUIP_ELEMENT_STAMINA = 0;
        /// <summary>
        /// Max Stamina.
        /// </summary>
        public const int EQUIP_ELEMENT_MAX_STAMINA = 1;
        /// <summary>
        /// Skill.
        /// </summary>
        public const int EQUIP_ELEMENT_SKILL = 2;
        /// <summary>
        /// Max Skill.
        /// </summary>
        public const int EQUIP_ELEMENT_MAX_SKILL = 3;
        /// <summary>
        /// Luck.
        /// </summary>
        public const int EQUIP_ELEMENT_LUCK = 4;
        /// <summary>
        /// Max Luck.
        /// </summary>
        public const int EQUIP_ELEMENT_MAX_LUCK = 5;
        /// <summary>
        /// Damage.
        /// </summary>
        public const int EQUIP_ELEMENT_DAMAGE = 6;
        /// <summary>
        /// the number of equipment element modifiers that exist.
        /// </summary>
        public const int NUM_ELEMENTS = 7;
        //****************************
        // IO FLAGS
        //****************************
        /// <summary>
        /// flag indicating the IO is a door.
        /// </summary>
        public const int IO_17_DOOR = 1 << 17;
        //****************************
        // SCRIPT MESSAGES
        //****************************
        public const int SM_300_ROLL_STATS = 300;
        /// <summary>
        /// IO entered a trigger area.
        /// </summary>
        public const int SM_301_TRIGGER_ENTER = 301;
        /// <summary>
        /// IO was bashed.
        /// </summary>
        public const int SM_303_BASHED = 303;
        /// <summary>
        /// IO is out of view.
        /// </summary>
        public const int SM_304_OUT_OF_VIEW = 304;
        /// <summary>
        /// IO is out of view.
        /// </summary>
        public const int SM_305_IN_VIEW = 305;
    }
}