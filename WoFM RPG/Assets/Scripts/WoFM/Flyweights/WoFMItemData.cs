using RPGBase.Flyweights;
using System;

namespace WoFM.Flyweights
{
    public class WoFMItemData : IOItemData
    {
        /// <summary>
        /// The item's sprite.
        /// </summary>
        public string Sprite { get; set; }
        /// <summary>
        /// The item's sprite's initial rotation.  Some images are rotated 45 degrees.
        /// </summary>
        public float SpriteRotation { get; set; }

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