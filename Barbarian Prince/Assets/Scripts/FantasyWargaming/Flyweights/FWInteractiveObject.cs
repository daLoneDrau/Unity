using RPGBase.Flyweights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.FantasyWargaming.Flyweights
{
    public class FWInteractiveObject : BaseInteractiveObject
    {
        public FWInteractiveObject(int id):base(id)
        {
            Inventory = new FWInventoryData();
            ItemData = new FWItemData();
        }
    }
}
