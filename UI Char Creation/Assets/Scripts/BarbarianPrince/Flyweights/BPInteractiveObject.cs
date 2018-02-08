using RPGBase.Flyweights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.BarbarianPrince.Flyweights
{
    public class BPInteractiveObject : BaseInteractiveObject
    {
        public BPInteractiveObject(int id):base(id)
        {
            Inventory = new BPInventoryData();
            ItemData = new BPItemData();
        }
    }
}
