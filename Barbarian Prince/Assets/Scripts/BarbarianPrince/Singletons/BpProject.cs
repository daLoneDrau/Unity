using Assets.Scripts.BarbarianPrince.Constants;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.BarbarianPrince.Singletons
{
    public class BPProject : ProjectConstants
    {
        public BPProject()
        {
            if (ProjectConstants.Instance == null)
            {
                ProjectConstants.Instance = this;
            }
        }
        public override int GetDamageElementIndex()
        {
            throw new NotImplementedException();
        }

        public override int GetMaxEquipped()
        {
            return BPGlobals.MAX_EQUIPPED;
        }

        public override int GetMaxSpells()
        {
            throw new NotImplementedException();
        }

        public override int GetNumberEquipmentElements()
        {
            return BPGlobals.NUM_ELEMENTS;
        }

        public override int GetPlayer()
        {
            throw new NotImplementedException();
        }
    }
}
