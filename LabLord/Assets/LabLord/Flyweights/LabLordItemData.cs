using RPGBase.Flyweights;
using System;

namespace LabLord.Flyweights
{
    public class LabLordItemData : IOItemData
    {
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