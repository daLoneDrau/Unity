using RPGBase.Flyweights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Blueholme.Flyweights
{
    public class BHInteractiveObject : BaseInteractiveObject
    {
        public BHInteractiveObject(int id):base(id)
        {
            Inventory = new BHInventoryData();
            ItemData = new BHItemData();
        }
    }
}
