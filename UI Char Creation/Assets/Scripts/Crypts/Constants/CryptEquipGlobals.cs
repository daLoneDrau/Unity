using RPGBase.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Crypts.Constants
{
    public sealed class CryptEquipGlobals : EquipmentGlobals
    {
        public const int EQUIP_ELEMENT_STRENGTH = 0;
        public const int EQUIP_ELEMENT_DEXTERITY = 1;
        public const int EQUIP_ELEMENT_CONSTITUTION = 2;
        public const int EQUIP_ELEMENT_INTELLIGENCE = 3;
        public const int EQUIP_ELEMENT_WISDOM = 4;
        public const int EQUIP_ELEMENT_CHARISMA = 5;
        public const int EQUIP_ELEMENT_LUCK = 6;
        public const int EQUIP_ELEMENT_SKILL = 7;
        public const int EQUIP_ELEMENT_AC = 8;
        public const int EQUIP_ELEMENT_HP = 9;
        public const int EQUIP_ELEMENT_MHP = 10;
        public const int EQUIP_ELEMENT_CORRUPTION = 11;
        public const int EQUIP_ELEMENT_SANITY = 12;
        public const int EQUIP_ELEMENT_TO_HIT = 13;
        public const int EQUIP_ELEMENT_DMG_BONUS = 14;
        public const int EQUIP_ELEMENT_MISSILE_BONUS = 15;
        public const int EQUIP_ELEMENT_AC_MODIFIER = 16;
        public const int EQUIP_ELEMENT_HP_BONUS = 17;
        public const int EQUIP_ELEMENT_LANGUAGE = 18;
        public const int EQUIP_ELEMENT_CHARM = 19;
        public const int EQUIP_ELEMENT_MAX_HIRELINGS = 20;

        public const int CLASS_BARBARIAN = 1 << 0;
        public const int CLASS_FIGHTER = 1 << 1;
        public const int CLASS_SORCERER = 1 << 2;
        public const int CLASS_THIEF = 1 << 3;

        public const int UNARMOURED_AC = 10;


        public const int NUM_EQUIP_ELEMENTS = 21;
        public const int MAX_EQUIPPED = 6;
    }
}
