using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPGBase.Singletons;

namespace RPGTests.Singletons
{
    public class TestProjectConstants : ProjectConstants
    {
        public TestProjectConstants()
        {
            base.SetInstance(this);
        }
        public override int GetDamageElementIndex()
        {
            return 0;
        }

        public override int GetMaxEquipped()
        {
            return 8;
        }

        public override int GetMaxSpells()
        {
            return 0;
        }

        public override int GetNumberEquipmentElements()
        {
            return 2;
        }

        public override int GetPlayer()
        {
            return 0;
        }
    }
}
