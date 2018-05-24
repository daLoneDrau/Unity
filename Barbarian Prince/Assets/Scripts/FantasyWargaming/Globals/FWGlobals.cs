using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPGBase.Constants;

namespace Assets.Scripts.FantasyWargaming.Globals
{
    public sealed class FWGlobals
    {
        //****************************
        // EQUIPMENT ELEMENTS
        //****************************
        /// <summary>
        /// Aglity.
        /// </summary>
        public const int EQUIP_ELEMENT_AGI = 0;
        /// <summary>
        /// Bravery.
        /// </summary>
        public const int EQUIP_ELEMENT_BRV = 1;
        /// <summary>
        /// Charisma.
        /// </summary>
        public const int EQUIP_ELEMENT_CHA = 2;
        /// <summary>
        /// Endurance.
        /// </summary>
        public const int EQUIP_ELEMENT_END = 3;
        /// <summary>
        /// Faith.
        /// </summary>
        public const int EQUIP_ELEMENT_FTH = 4;
        /// <summary>
        /// Greed.
        /// </summary>
        public const int EQUIP_ELEMENT_GRE = 5;
        /// <summary>
        /// Intelligence.
        /// </summary>
        public const int EQUIP_ELEMENT_INT = 6;
        /// <summary>
        /// Lust.
        /// </summary>
        public const int EQUIP_ELEMENT_LUS = 7;
        /// <summary>
        /// Mana.
        /// </summary>
        public const int EQUIP_ELEMENT_MAN = 8;
        /// <summary>
        /// Physique.
        /// </summary>
        public const int EQUIP_ELEMENT_PHY = 9;
        /// <summary>
        /// Piety.
        /// </summary>
        public const int EQUIP_ELEMENT_PIE = 10;
        /// <summary>
        /// Selfishness.
        /// </summary>
        public const int EQUIP_ELEMENT_SEL = 11;
        /// <summary>
        /// Social Class.
        /// </summary>
        public const int EQUIP_ELEMENT_SOC = 12;
        /// <summary>
        /// Leadership.
        /// </summary>
        public const int EQUIP_ELEMENT_LEA = 13;
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
