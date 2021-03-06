﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGBase.Constants
{
    public sealed class IoGlobals
    {
        public const int GFLAG_DOOR = 1 << 5;
        public const int GFLAG_ELEVATOR = 1 << 10;
        public const int GFLAG_GOREEXPLODE = 1 << 14;
        public const int GFLAG_HIDEWEAPON = 1 << 12;
        public const int GFLAG_INTERACTIVITY = 1;
        public const int GFLAG_INTERACTIVITYHIDE = 1 << 4;
        public const int GFLAG_INVISIBILITY = 1 << 6;
        public const int GFLAG_ISINTREATZONE = 1 << 1;
        public const int GFLAG_MEGAHIDE = 1 << 11;
        public const int GFLAG_NEEDINIT = 1 << 3;
        public const int GFLAG_NO_PHYS_IO_COL = 1 << 7;
        public const int GFLAG_NOCOMPUTATION = 1 << 15;
        public const int GFLAG_NOGORE = 1 << 13;
        public const int GFLAG_PLATFORM = 1 << 9;
        public const int GFLAG_VIEW_BLOCKER = 1 << 8;
        public const int GFLAG_WASINTREATZONE = 1 << 2;
        /// <summary>
        /// flag indicating the interactive object is a IOPcData.
        /// </summary>
        public const int IO_01_PC = 1;
        /// <summary>
        /// flag indicating the interactive object is an item.
        /// </summary>
        public const int IO_02_ITEM = 2;
        /// <summary>
        /// flag indicating the interactive object is an IONpcData.
        /// </summary>
        public const int IO_03_NPC = 4;
        /** flag indicating the interactive object is a horse object. */
        public const int IO_04_HORSE = 8;
        /** flag indicating the interactive object is under water. */
        public const int IO_05_UNDERWATER = 16;
        /** flag indicating the interactive object is a fixable object. */
        public const int IO_06_FREEZESCRIPT = 32;
        /** flag indicating the interactive object is a fixable object. */
        public const int IO_07_NO_COLLISIONS = 64;
        /** flag indicating the interactive object is a fixable object. */
        public const int IO_08_INVULNERABILITY = 128;
        /** flag indicating the interactive object is a dwelling. */
        public const int IO_09_DWELLING = 256;
        /** flag indicating the interactive object is gold. */
        public const int IO_10_GOLD = 512;
        /** flag indicating the interactive object has interactivity. */
        public const int IO_11_INTERACTIVITY = 1024;
        /** flag indicating the interactive object is a treasure. */
        public const int IO_12_TREASURE = 2048;
        /** flag indicating the interactive object is unique. */
        public const int IO_13_UNIQUE = 4096;
        /** flag indicating the interactive object is a party. */
        public const int IO_14_PARTY = 8192;
        /** flag indicating the interactive object is a winged mount. */
        public const int IO_15_MOVABLE = 16384;
        /** flag indicating the interactive object is a trigger. */
        public const int IO_16_TRIGGER = 1 << 16;
        /** flag indicating the interactive object is fixable. */
        public const int IO_17_FIX = 1 << 17;
        /// <summary>
        /// flag indicating that an NPC can be backstabbed
        /// </summary>
        public const int NPCFLAG_BACKSTAB = 1;
        public const int PLAYER_CROUCH = 1 << 7;
        public const int PLAYER_LEAN_LEFT = 1 << 8;
        public const int PLAYER_LEAN_RIGHT = 1 << 9;
        public const int PLAYER_MOVE_JUMP = 1 << 4;
        public const int PLAYER_MOVE_STEALTH = 1 << 5;
        public const int PLAYER_MOVE_STRAFE_LEFT = 1 << 2;
        public const int PLAYER_MOVE_STRAFE_RIGHT = 1 << 3;
        public const int PLAYER_MOVE_WALK_BACKWARD = 1 << 1;
        public const int PLAYER_MOVE_WALK_FORWARD = 1;
        public const int PLAYER_ROTATE = 1 << 6;
        public const int PLAYERFLAGS_INVULNERABILITY = 2;
        public const int PLAYERFLAGS_NO_MANA_DRAIN = 1;
        public const int SHOW_FLAG_DESTROYED = 255;
        public const int SHOW_FLAG_HIDDEN = 5;       // In =
                                                      // Inventory;
        public const int SHOW_FLAG_IN_INVENTORY = 4;     // In =
                                                          // Inventory;
        public const int SHOW_FLAG_IN_SCENE = 1;
        public const int SHOW_FLAG_KILLED = 7;       // Not Used
                                                      // = Yet;
        public const int SHOW_FLAG_LINKED = 2;
        public const int SHOW_FLAG_MEGAHIDE = 8;
        public const int SHOW_FLAG_NOT_DRAWN = 0;
        public const int SHOW_FLAG_ON_PLAYER = 9;
        public const int SHOW_FLAG_TELEPORTING = 6;
        public const int TARGET_PATH = -3;
        public const int TARGET_NONE = -2;
        public const int TARGET_PLAYER = 0;
        public const int WALKMODE = 0;
        public const int RUNMODE = 1;
        public const int NOMOVEMODE = 2;
        public const int SNEAKMODE = 3;
        public const int NO_IDENT = 1;
        public const int NO_MESH = 2;
        public const int NO_ON_LOAD = 4;
        public const int IO_IMMEDIATELOAD = 8;
        /// <summary>
        /// Hidden constructor.
        /// </summary>
        private IoGlobals()
        {
        }
    }
}
