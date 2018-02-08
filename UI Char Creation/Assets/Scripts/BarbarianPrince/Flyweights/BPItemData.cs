using RPGBase.Flyweights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.BarbarianPrince.Flyweights
{
    public class BPItemData : IOItemData
    {
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
