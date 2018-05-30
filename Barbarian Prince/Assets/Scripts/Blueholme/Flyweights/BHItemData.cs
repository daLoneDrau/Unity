using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Blueholme.Flyweights
{
    public class BHItemData : IOItemData
    {
        /// <summary>
        /// the dice rolled to calculate the weapon's damage.
        /// </summary>
        public Dice Dice { get; set; }
        /// <summary>
        /// the modifier added to the dice roll to get a weapon's damage.
        /// </summary>
        public int DmgModifier { get; set; }
        /// <summary>
        /// The weapon's speed.
        /// </summary>
        public int WeaponSpeed { get; set; }

        protected override float ApplyCriticalModifier()
        {
            throw new NotImplementedException();
        }

        protected override float CalculateArmorDeflection()
        {
            throw new NotImplementedException();
        }
        public override float GetBackstabModifier()
        {
            throw new NotImplementedException();
        }
    }
}
