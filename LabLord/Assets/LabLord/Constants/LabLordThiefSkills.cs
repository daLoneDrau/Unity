using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabLord.Constants
{
    public sealed class LabLordThiefSkills
    {
        /// <summary>
        /// THIEF PICK LOCKS
        /// </summary>
        public const int PICK_LOCKS = 0;
        /// <summary>
        /// THIEF FIND AND REMOVE TRAPS
        /// </summary>
        public const int FIND_REMOVE_TRAPS = 1;
        /// <summary>
        /// THIEF PICK POCKETS
        /// </summary>
        public const int PICK_POCKETS = 2;
        /// <summary>
        /// THIEF MOVE SILENTLY
        /// </summary>
        public const int MOVE_SILENTLY = 3;
        /// <summary>
        /// THIEF CLIMB WALLS
        /// </summary>
        public const int CLIMB_WALLS = 4;
        /// <summary>
        /// THIEF HIDE IN SHADOWS
        /// </summary>
        public const int HIDE_SHADOWS = 5;
        /// <summary>
        /// THIEF HEAR NOISE
        /// </summary>
        public const int HEAR_NOISE = 6;
        public static int[] EQUIP_ELEMENT_MAP = {
            LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_LOCKS,
            LabLordGlobals.EQUIP_ELEMENT_THIEF_FIND_REMOVE_TRAPS,
            LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_POCKETS,
            LabLordGlobals.EQUIP_ELEMENT_THIEF_MOVE_SILENTLY,
            LabLordGlobals.EQUIP_ELEMENT_THIEF_CLIMB_WALLS,
            LabLordGlobals.EQUIP_ELEMENT_THIEF_HIDE_SHADOWS,
            LabLordGlobals.EQUIP_ELEMENT_THIEF_HEAR_NOISE
        };
        /// <summary>
        /// Thief Skill titles.
        /// </summary>
        public static string[] THIEF_SKILL_TITLES = {
            "Pick Locks",
            "Find and Remove Traps",
            "Pick Pockets",
            "Move Silently",
            "Climb Walls",
            "Hide Shadows",
            "Hear Noise"
        };
        public static string[] TAB_LIST = {
            "\t\t\t\t\t\t",
            "\t",
            "\t\t\t\t\t",
            "\t\t\t\t",
            "\t\t\t\t\t",
            "\t\t\t\t",
            "\t\t\t\t\t\t"
        };
    }
}
