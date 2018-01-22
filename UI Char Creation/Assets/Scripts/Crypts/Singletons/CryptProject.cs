using Assets.Scripts.Crypts.Constants;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Crypts.Singletons
{
    public class CryptProject : ProjectConstants
    {
        public CryptProject()
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
            return CryptEquipGlobals.MAX_EQUIPPED;
        }

        public override int GetMaxSpells()
        {
            throw new NotImplementedException();
        }

        public override int GetNumberEquipmentElements()
        {
            return CryptEquipGlobals.NUM_EQUIP_ELEMENTS;
        }

        public override int GetPlayer()
        {
            throw new NotImplementedException();
        }
    }
}
