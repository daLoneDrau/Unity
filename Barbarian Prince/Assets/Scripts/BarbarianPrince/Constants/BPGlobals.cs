using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.BarbarianPrince.Constants
{
    public sealed class BPGlobals
    {
        //****************************
        // EQUIPMENT SLOTS
        //****************************
        /// <summary>
        /// left ring slot.
        /// </summary>
        public const int EQUIP_SLOT_WEAPON = 0;
        /// <summary>
        /// The # of equipment slots.
        /// </summary>
        public const int MAX_EQUIPPED = 1;
        //****************************
        // ELEMENTS
        //****************************
        /// <summary>
        /// combat skill.
        /// </summary>
        public const int ELEMENT_COMBAT_SKILL = 0;
        /// <summary>
        /// endurance.
        /// </summary>
        public const int ELEMENT_ENDURANCE = 1;
        /// <summary>
        /// wealth.
        /// </summary>
        public const int ELEMENT_WEALTH = 2;
        /// <summary>
        /// wounds.
        /// </summary>
        public const int ELEMENT_WOUNDS = 3;
        /// <summary>
        /// poison wounds.
        /// </summary>
        public const int ELEMENT_POISON_WOUNDS = 4;
        /// <summary>
        /// wit and wiles.
        /// </summary>
        public const int ELEMENT_WIT_AND_WILES = 5;
        /// <summary>
        /// The number of element modifiers.
        /// </summary>
        public const int NUM_ELEMENTS = 6;
        //****************************
        // OBJECT TYPES
        //****************************
        /// <summary>
        /// weapon objects.
        /// </summary>
        public const int OBJECT_TYPE_WEAPON = 1 << 0;
        /// <summary>
        /// food objects.
        /// </summary>
        public const int OBJECT_TYPE_FOOD = 1 << 1;
        /// <summary>
        /// 1-handed weapon objects.
        /// </summary>
        public const int OBJECT_TYPE_1H = 1 << 2;
        /// <summary>
        /// gold objects.
        /// </summary>
        public const int OBJECT_TYPE_GOLD = 1 << 3;
        //****************************
        // TURN ACTIONS
        //****************************
        /// <summary>
        /// airborne travel.
        /// </summary>
        public const int ACTION_AIR_TRAVEL = 8;
        /// <summary>
        /// seek audience with local lord in town, castle or temple.
        /// </summary>
        public const int ACTION_AUDIENCE = 128;
        /// <summary>
        /// seek to hire followers in town or castle.
        /// </summary>
        public const int ACTION_HIRE = 64;
        /// <summary>
        /// travel overland.
        /// </summary>
        public const int ACTION_LAND_TRAVEL = 2;
        /// <summary>
        /// seek news & information in town, castle, or temple.
        /// </summary>
        public const int ACTION_NEWS = 32;
        /// <summary>
        /// submit offering at temple.
        /// </summary>
        public const int ACTION_OFFERING = 256;
        //***************** Daily actions
        /// <summary>
        /// rest in current hex, to heal wounds and improve hunting.
        /// </summary>
        public const int ACTION_REST = 1;
        /// <summary>
        /// waterborne travel.
        /// </summary>
        public const int ACTION_RIVER_TRAVEL = 4;
        /// <summary>
        /// search for a previously placed cache.
        /// </summary>
        public const int ACTION_SEARCH_CACHE = 16;
        /// <summary>
        /// search in a ruins.
        /// </summary>
        public const int ACTION_SEARCH_RUINS = 512;
        /// <summary>
        /// leaves items or gold in a "hidden cache".
        /// </summary>
        public const int ACTION_MAKE_CACHE = 1024;
        /// <summary>
        /// master script message - time change.
        /// </summary>
        public const int SM_300_TIME_CHANGE = 300;
        /// <summary>
        /// combat skill.
        /// </summary>
        public const int EQUIP_ELEMENT_CS = 0;
        /// <summary>
        /// endurance.
        /// </summary>
        public const int EQUIP_ELEMENT_EN = 1;
        /// <summary>
        /// wounds.
        /// </summary>
        public const int EQUIP_ELEMENT_WO = 2;
        /// <summary>
        /// poison wounds.
        /// </summary>
        public const int EQUIP_ELEMENT_PW = 3;
        /// <summary>
        /// wealth.
        /// </summary>
        public const int EQUIP_ELEMENT_WE = 4;
        /// <summary>
        /// wit & wiles.
        /// </summary>
        public const int EQUIP_ELEMENT_WI = 5;
    }
}
