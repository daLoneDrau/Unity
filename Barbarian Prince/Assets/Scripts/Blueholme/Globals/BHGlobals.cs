using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPGBase.Constants;

namespace Assets.Scripts.Blueholme.Globals
{
    public sealed class BHGlobals
    {
        //****************************
        // EQUIPMENT ELEMENTS
        //****************************
        /// <summary>
        /// Dexterity.
        /// </summary>
        public const int EQUIP_ELEMENT_DEX = 0;
        /// <summary>
        /// Charisma.
        /// </summary>
        public const int EQUIP_ELEMENT_CHA = 1;
        /// <summary>
        /// Constitution.
        /// </summary>
        public const int EQUIP_ELEMENT_CON = 2;
        /// <summary>
        /// Hit Points.
        /// </summary>
        public const int EQUIP_ELEMENT_HP = 3;
        /// <summary>
        /// Intelligence.
        /// </summary>
        public const int EQUIP_ELEMENT_INT = 4;
        /// <summary>
        /// Max Hit Points.
        /// </summary>
        public const int EQUIP_ELEMENT_MHP = 5;
        /// <summary>
        /// Save vs Breath Weapon
        /// </summary>
        public const int EQUIP_ELEMENT_SBW = 6;
        /// <summary>
        /// Save vs Gaze
        /// </summary>
        public const int EQUIP_ELEMENT_SGZ = 7;
        /// <summary>
        /// Save vs Magic Wand
        /// </summary>
        public const int EQUIP_ELEMENT_SMW = 8;
        /// <summary>
        /// Save vs Ray or Poison
        /// </summary>
        public const int EQUIP_ELEMENT_SRP = 9;
        /// <summary>
        /// Save vs Staff or Spell
        /// </summary>
        public const int EQUIP_ELEMENT_SSS = 10;
        /// <summary>
        /// Strength.
        /// </summary>
        public const int EQUIP_ELEMENT_STR = 11;
        /// <summary>
        /// Wisdom.
        /// </summary>
        public const int EQUIP_ELEMENT_WIS = 12;
        /// <summary>
        /// Dexterity.
        /// </summary>
        public const int EQUIP_ELEMENT_AC = 13;
        /// <summary>
        /// the number of equipment slots
        /// <para>0 - Left hand ring</para>
        /// <para>1 - Right hand ring</para>
        /// <para>2 - weapon</para>
        /// <para>3 - shield</para>
        /// <para>4 - torch</para>
        /// <para>5 - torso</para>
        /// <para>6 - helmet</para>
        /// <para>7 - leggings</para>
        /// see complete list in <see cref="EquipmentGlobals"/>
        /// </summary>
        public const int MAX_EQUIPPED=8;
        public const int NUM_ELEMENTS=14;
        /// <summary>
        /// script message morale check
        /// </summary>
        public const int SM_300_MORALE_CHECK = 300;
        /// <summary>
        /// script message berserk check
        /// </summary>
        public const int SM_301_BERSERK_CHECK = 301;
        /// <summary>
        /// script message combat flurry
        /// </summary>
        public const int SM_302_COMBAT_FLURRY = 302;
    }
}
