using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Avalon.Constants
{
    public sealed class BPGlobals
    {
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
        // EQUIPMENT SLOTS
        //****************************
        /// <summary>
        /// weapon slot.
        /// </summary>
        public const int EQUIP_SLOT_WEAPON = 0;
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
        /// gold objects.
        /// </summary>
        public const int OBJECT_TYPE_GOLD = 1 << 2;
    }
}
