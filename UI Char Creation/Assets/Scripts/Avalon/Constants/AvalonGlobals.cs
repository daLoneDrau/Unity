using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Avalon.Constants
{
    public sealed class AvalonGlobals
    {
        //****************************
        // EQUIPMENT SLOTS
        //****************************
        /// <summary>
        /// left ring slot.
        /// </summary>
        public const int EQUIP_SLOT_RING_LEFT = 0;
        /// <summary>
        /// right ring slot.
        /// </summary>
        public const int EQUIP_SLOT_RING_RIGHT = 1;
        /// <summary>
        /// weapon slot.
        /// </summary>
        public const int EQUIP_SLOT_WEAPON = 2;
        /// <summary>
        /// shield slot.
        /// </summary>
        public const int EQUIP_SLOT_SHIELD = 3;
        /// <summary>
        /// breastplate slot.
        /// </summary>
        public const int EQUIP_SLOT_BREASTPLATE = 4;
        /// <summary>
        /// helmet slot.
        /// </summary>
        public const int EQUIP_SLOT_HELMET = 5;
        /// <summary>
        /// armour slot.
        /// </summary>
        public const int EQUIP_SLOT_ARMOUR = 6;
        /// <summary>
        /// The # of equipment slots.
        /// </summary>
        public const int MAX_EQUIPPED = 7;
        //****************************
        // OBJECT TYPES
        //****************************
        /// <summary>
        /// weapon objects.
        /// </summary>
        public const int OBJECT_TYPE_WEAPON = 1 << 0;
        /// <summary>
        /// 1-handed weapon objects.
        /// </summary>
        public const int OBJECT_TYPE_1H = 1 << 1;
        /// <summary>
        /// 2-handed weapon objects.
        /// </summary>
        public const int OBJECT_TYPE_2H = 1 << 2;
        /// <summary>
        /// bow objects.
        /// </summary>
        public const int OBJECT_TYPE_BOW = 1 << 3;
        /// <summary>
        /// armour objects.
        /// </summary>
        public const int OBJECT_TYPE_ARMOR = 1 << 4;
        /// <summary>
        /// suit of armor objects.
        /// </summary>
        public const int OBJECT_TYPE_SUIT_OF_ARMOR = 1 << 5;
        /// <summary>
        /// breastplate objects.
        /// </summary>
        public const int OBJECT_TYPE_BREASTPLATE = 1 << 6;
        /// <summary>
        /// helmet objects.
        /// </summary>
        public const int OBJECT_TYPE_HELMET = 1 << 7;
        /// <summary>
        /// shield objects.
        /// </summary>
        public const int OBJECT_TYPE_SHIELD = 1 << 8;
        /// <summary>
        /// boots objects.
        /// </summary>
        public const int OBJECT_TYPE_BOOTS = 1 << 9;
        /// <summary>
        /// gloves objects.
        /// </summary>
        public const int OBJECT_TYPE_GLOVES = 1 << 10;
        /// <summary>
        /// potions objects.
        /// </summary>
        public const int OBJECT_TYPE_POTION = 1 << 11;
        /// <summary>
        /// cloak objects.
        /// </summary>
        public const int OBJECT_TYPE_CLOAK = 1 << 12;
        /// <summary>
        /// necklace objects.
        /// </summary>
        public const int OBJECT_TYPE_NECKLACE = 1 << 13;
        /// <summary>
        /// book objects.
        /// </summary>
        public const int OBJECT_TYPE_BOOK = 1 << 14;
        /// <summary>
        /// belt objects.
        /// </summary>
        public const int OBJECT_TYPE_BELT = 1 << 15;
        /// <summary>
        /// artifact objects.
        /// </summary>
        public const int OBJECT_TYPE_ARTIFACT = 1 << 16;
        /// <summary>
        /// ring objects.
        /// </summary>
        public const int OBJECT_TYPE_RING = 1 << 17;
        /// <summary>
        /// focus objects.
        /// </summary>
        public const int OBJECT_TYPE_FOCUS = 1 << 18;
        /// <summary>
        /// bracers objects.
        /// </summary>
        public const int OBJECT_TYPE_BRACERS = 1 << 19;
        /// <summary>
        /// map objects.
        /// </summary>
        public const int OBJECT_TYPE_MAP = 1 << 20;
        /// <summary>
        /// magic item objects.
        /// </summary>
        public const int OBJECT_TYPE_MAGIC_ITEM = 1 << 21;
        /// <summary>
        /// miscellaneous objects.
        /// </summary>
        public const int OBJECT_TYPE_MISC = 1 << 22;
    }
}
