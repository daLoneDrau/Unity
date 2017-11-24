using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGBase.Constants
{
    public class ScriptConsts
    {
        public const int DISABLE_AGGRESSION = 32;
        public const int DISABLE_CHAT = 2;
        public const int DISABLE_COLLIDE_NPC = 128;
        public const int DISABLE_CURSORMODE = 256;
        public const int DISABLE_DETECT = 16;
        public const int DISABLE_EXPLORATIONMODE = 512;
        public const int DISABLE_HEAR = 8;
        public const int DISABLE_HIT = 1;
        public const int DISABLE_INVENTORY2_OPEN = 4;
        public const int DISABLE_MAIN = 64;
        public const int REFUSE = -1;
        public const int SM_000_NULL = 0;
        public const int SM_001_INIT = 1;
        public const int SM_002_INVENTORYIN = 2;
        public const int SM_003_INVENTORYOUT = 3;
        public const int SM_004_INVENTORYUSE = 4;
        public const int SM_005_SCENEUSE = 5;
        public const int SM_006_EQUIPIN = 6;
        public const int SM_007_EQUIPOUT = 7;
        public const int SM_008_MAIN = 8;
        public const int SM_09_RESET = 9;
        public const int SM_10_CHAT = 10;
        public const int SM_11_ACTION = 11;
        public const int SM_12_DEAD = 12;
        public const int SM_13_REACHEDTARGET = 13;
        public const int SM_14_FIGHT = 14;
        public const int SM_15_FLEE = 15;
        /** script message - the BaseInteractiveObject has been hit. */
        public const int SM_016_HIT = 16;
        public const int SM_017_DIE = 17;
        public const int SM_18_LOSTTARGET = 18;
        public const int SM_19_TREATIN = 19;
        public const int SM_20_TREATOUT = 20;
        /** script message to move to a travel location. */
        public const int SM_21_MOVE = 21;
        public const int SM_22_DETECTPLAYER = 22;
        public const int SM_23_UNDETECTPLAYER = 23;
        public const int SM_24_COMBINE = 24;
        public const int SM_25_NPC_FOLLOW = 25;
        public const int SM_255_EXECUTELINE = 255;
        public const int SM_256_DUMMY = 256;
        public const int SM_26_NPC_FIGHT = 26;
        public const int SM_27_NPC_STAY = 27;
        public const int SM_28_INVENTORY2_OPEN = 28;
        public const int SM_29_INVENTORY2_CLOSE = 29;
        public const int SM_30_CUSTOM = 30;
        public const int SM_31_ENTER_ZONE = 31;
        public const int SM_32_LEAVE_ZONE = 32;
        public const int SM_033_INITEND = 33;
        public const int SM_34_CLICKED = 34;
        public const int SM_35_INSIDEZONE = 35;
        public const int SM_36_CONTROLLEDZONE_INSIDE = 36;
        public const int SM_37_LEAVEZONE = 37;
        public const int SM_38_CONTROLLEDZONE_LEAVE = 38;
        public const int SM_39_ENTERZONE = 39;
        public const int SM_40_CONTROLLEDZONE_ENTER = 40;
        public const int SM_41_LOAD = 41;
        public const int SM_42_SPELLCAST = 42;
        public const int SM_43_RELOAD = 43;
        public const int SM_44_COLLIDE_DOOR = 44;
        public const int SM_045_OUCH = 45;
        public const int SM_46_HEAR = 46;
        public const int SM_47_SUMMONED = 47;
        public const int SM_48_SPELLEND = 48;
        public const int SM_49_SPELLDECISION = 49;
        public const int SM_50_STRIKE = 50;
        public const int SM_51_COLLISION_ERROR = 51;
        public const int SM_52_WAYPOINT = 52;
        public const int SM_53_PATHEND = 53;
        public const int SM_54_CRITICAL = 54;
        public const int SM_55_COLLIDE_NPC = 55;
        public const int SM_56_BACKSTAB = 56;
        public const int SM_57_AGGRESSION = 57;
        public const int SM_58_COLLISION_ERROR_DETAIL = 58;
        public const int SM_59_GAME_READY = 59;
        public const int SM_60_CINE_END = 60;
        public const int SM_61_KEY_PRESSED = 61;
        public const int SM_62_CONTROLS_ON = 62;
        public const int SM_63_CONTROLS_OFF = 63;
        public const int SM_64_PATHFINDER_FAILURE = 64;
        public const int SM_65_PATHFINDER_SUCCESS = 65;
        public const int SM_66_TRAP_DISARMED = 66;
        public const int SM_67_BOOK_OPEN = 67;
        public const int SM_68_BOOK_CLOSE = 68;
        public const int SM_69_IDENTIFY = 69;
        public const int SM_70_BREAK = 70;
        public const int SM_71_STEAL = 71;
        public const int SM_72_COLLIDE_FIELD = 72;
        public const int SM_73_CURSORMODE = 73;
        public const int SM_74_EXPLORATIONMODE = 74;
        public const int SM_75_MAXCMD = 75;
        public const int ACCEPT = 1;
        public const int MAX_EVENT_STACK = 800;
        public const int MAX_SCRIPTTIMERS = 5;
        /** flag indicating the script variable is a global string. */
        public const int TYPE_G_00_TEXT = 0;
        /** flag indicating the script variable is a global string. */
        public const int TYPE_G_01_TEXT_ARR = 1;
        /** flag indicating the script variable is a global floating-point type. */
        public const int TYPE_G_02_FLOAT = 2;
        /** flag indicating the script variable is a global floating-point array. */
        public const int TYPE_G_03_FLOAT_ARR = 3;
        /** flag indicating the script variable is a global integer. */
        public const int TYPE_G_04_INT = 4;
        /** flag indicating the script variable is a global integer array. */
        public const int TYPE_G_05_INT_ARR = 5;
        /** flag indicating the script variable is a global integer. */
        public const int TYPE_G_06_LONG = 6;
        /** flag indicating the script variable is a global long array. */
        public const int TYPE_G_07_LONG_ARR = 7;
        /** flag indicating the script variable is a local string. */
        public const int TYPE_L_08_TEXT = 8;
        /** flag indicating the script variable is a local string array. */
        public const int TYPE_L_09_TEXT_ARR = 9;
        /** flag indicating the script variable is a local floating-point type. */
        public const int TYPE_L_10_FLOAT = 10;
        /** flag indicating the script variable is a local floating-point array. */
        public const int TYPE_L_11_FLOAT_ARR = 11;
        /** flag indicating the script variable is a local integer. */
        public const int TYPE_L_12_INT = 12;
        /** flag indicating the script variable is a local integer array. */
        public const int TYPE_L_13_INT_ARR = 13;
        /** flag indicating the script variable is a local integer. */
        public const int TYPE_L_14_LONG = 14;
        /** flag indicating the script variable is a local long array. */
        public const int TYPE_L_15_LONG_ARR = 15;
        /**
         * Creates a new instance of {@link ScriptConsts}.
         */
        protected ScriptConsts()
        {
        }
    }
}
