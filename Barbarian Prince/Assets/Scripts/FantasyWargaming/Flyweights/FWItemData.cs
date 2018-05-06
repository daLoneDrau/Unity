using RPGBase.Constants;
using RPGBase.Flyweights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.FantasyWargaming.Flyweights
{
    public class FWItemData : IOItemData
    {
        /// <summary>
        /// the minimum physique needed to wield the item.
        /// </summary>
        public float MinMeleePhysique { get; set; }
        /// <summary>
        /// the minimum Agility needed to wield the item.
        /// </summary>
        public float MinMeleeAgility { get; set; }
        /// <summary>
        /// the item's striking range.
        /// </summary>
        public float Range { get; set; }
        public Dice Dice { get; set; }
        public int DmgModifier { get; set; }
        public bool Parry { get; set; }
        public int ParryModifier { get; set; }
        public char BreakCode { get; set; }
        protected override float ApplyCriticalModifier()
        {
            throw new NotImplementedException();
        }

        protected override float CalculateArmorDeflection()
        {
            throw new NotImplementedException();
        }
        protected override float GetBackstabModifier()
        {
            throw new NotImplementedException();
        }
    }
}
