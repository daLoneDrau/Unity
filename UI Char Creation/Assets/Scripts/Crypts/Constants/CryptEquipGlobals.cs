using RPGBase.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Crypts.Constants
{
    public sealed class CryptEquipGlobals : EquipmentGlobals
    {
        public const int EQUIP_ELEMENT_AC = 0;
        public const int EQUIP_ELEMENT_AC_MODIFIER = 1;
        public const int EQUIP_ELEMENT_STRENGTH = 2;
        public const int EQUIP_ELEMENT_WISDOM = 3;
        public const int EQUIP_ELEMENT_DEXTERITY = 4;
        public const int EQUIP_ELEMENT_CHARM = 5;
        public const int EQUIP_ELEMENT_CORRUPTION = 6;
        public const int EQUIP_ELEMENT_DMG_BONUS = 7;
        public const int EQUIP_ELEMENT_INTELLIGENCE = 8;
        public const int EQUIP_ELEMENT_MAX_HIRELINGS = 9;
        public const int EQUIP_ELEMENT_CONSTITUTION = 10;
        public const int EQUIP_ELEMENT_TO_HIT = 11;
        public const int EQUIP_ELEMENT_LANGUAGE = 12;
        public const int EQUIP_ELEMENT_CHARISMA = 13;
        public const int EQUIP_ELEMENT_LUCK = 14;
        public const int EQUIP_ELEMENT_MHP = 15;
        public const int EQUIP_ELEMENT_MISSILE_BONUS = 16;
        public const int EQUIP_ELEMENT_SANITY = 17;
        public const int EQUIP_ELEMENT_SKILL = 18;

        public const int CLASS_BARBARIAN = 1 << 0;
        public const int CLASS_FIGHTER = 1 << 1;
        public const int CLASS_SORCERER = 1 << 2;
        public const int CLASS_THIEF = 1 << 3;

        public const int UNARMOURED_AC = 10;


        public const int NUM_EQUIP_ELEMENTS = 19;
        public const int MAX_EQUIPPED = 6;
    }
}
