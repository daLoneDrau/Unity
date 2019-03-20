using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGBase.Constants
{
    public class EquipmentGlobals
    {
        public const int DAMAGE_TYPE_ACID = 1 << 9;
        public const int DAMAGE_TYPE_COLD = 1 << 3;
        public const int DAMAGE_TYPE_DRAIN_LIFE = 1 << 12;
        public const int DAMAGE_TYPE_DRAIN_MANA = 1 << 13;
        public const int DAMAGE_TYPE_FAKEFIRE = 1 << 15;
        public const int DAMAGE_TYPE_FIELD = 1 << 16;
        public const int DAMAGE_TYPE_FIRE = 1;
        public const int DAMAGE_TYPE_GAS = 1 << 5;
        public const int DAMAGE_TYPE_GENERIC = 0;
        public const int DAMAGE_TYPE_LIGHTNING = 1 << 2;
        public const int DAMAGE_TYPE_MAGICAL = 1 << 1;
        public const int DAMAGE_TYPE_METAL = 1 << 6;
        public const int DAMAGE_TYPE_NO_FIX = 1 << 17;
        public const int DAMAGE_TYPE_ORGANIC = 1 << 10;
        public const int DAMAGE_TYPE_PER_SECOND = 1 << 11;
        public const int DAMAGE_TYPE_POISON = 1 << 4;
        public const int DAMAGE_TYPE_PUSH = 1 << 14;
        public const int DAMAGE_TYPE_STONE = 1 << 8;
        public const int DAMAGE_TYPE_WOOD = 1 << 7;
        public const int EQUIP_SLOT_RING_LEFT = 0;
        public const int EQUIP_SLOT_RING_RIGHT = 1;
        public const int EQUIP_SLOT_WEAPON = 2;
        public const int EQUIP_SLOT_SHIELD = 3;
        public const int EQUIP_SLOT_TORCH = 4;
        public const int EQUIP_SLOT_TORSO = 5;
        public const int EQUIP_SLOT_HELMET = 6;
        public const int EQUIP_SLOT_LEGGINGS = 7;
        public const int MAX_EQUIPPED = 8;
        public const int MAX_SPELLS = 0;
        /// <summary>
        /// Dagger type.
        /// </summary>
        public const int OBJECT_TYPE_DAGGER = 1 << 1;
        public const int OBJECT_TYPE_1H = 1 << 2;
        public const int OBJECT_TYPE_2H = 1 << 3;
        public const int OBJECT_TYPE_BOW = 1 << 4;
        public const int OBJECT_TYPE_SHIELD = 1 << 5;
        public const int OBJECT_TYPE_FOOD = 1 << 6;
        public const int OBJECT_TYPE_GOLD = 1 << 7;
        public const int OBJECT_TYPE_ARMOR = 1 << 8;
        public const int OBJECT_TYPE_HELMET = 1 << 9;
        public const int OBJECT_TYPE_RING = 1 << 10;
        public const int OBJECT_TYPE_LEGGINGS = 1 << 11;
        public const int OBJECT_TYPE_WEAPON = 1;
        public const int TARGET_NONE = -2;
        public const int TARGET_PLAYER = -1;
        public const int WEAPON_1H = 2;
        public const int WEAPON_2H = 3;
        public const int WEAPON_BARE = 0;
        public const int WEAPON_BOW = 4;
        public const int WEAPON_DAGGER = 1;
    }
}
