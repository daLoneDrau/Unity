using Assets.Scripts.FantasyWargaming.Scriptables.Mobs;
using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.FantasyWargaming.Flyweights
{
    public class FWItemData : IOItemData
    {
        /// <summary>
        /// The item's armor protection.
        /// </summary>
        public float ArmourValue { get; internal set; }
        public char BreakCode { get; set; }
        /// <summary>
        /// the dice rolled to calculate the weapon's damage.
        /// </summary>
        public Dice Dice { get; set; }
        /// <summary>
        /// the modifier added to the dice roll to get a weapon's damage.
        /// </summary>
        public int DmgModifier { get; set; }
        /// <summary>
        /// Gets the flag indicating whether the item is armour that has a collar.
        /// </summary>
        public bool IsCollared { get; internal set; }
        /// <summary>
        /// Gets the flag indicating whether the item is a helmet that has a face.
        /// </summary>
        public bool IsFaced { get; internal set; }
        /// <summary>
        /// Gets the flag indicating whether the item is armour that has a skirt.
        /// </summary>
        public bool IsSkirted { get; internal set; }
        /// <summary>
        /// Gets the flag indicating whether the item is armour that has sleeves.
        /// </summary>
        public bool IsSleeved { get; internal set; }
        /// <summary>
        /// the minimum Agility needed to wield the item.
        /// </summary>
        public float MinMeleeAgility { get; set; }
        /// <summary>
        /// the minimum physique needed to wield the item.
        /// </summary>
        public float MinMeleePhysique { get; set; }
        public bool Parry { get; set; }
        public int ParryModifier { get; set; }
        /// <summary>
        /// the item's striking range.
        /// </summary>
        public float Range { get; set; }
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
