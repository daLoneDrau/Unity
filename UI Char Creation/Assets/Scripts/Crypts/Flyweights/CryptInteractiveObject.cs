using RPGBase.Flyweights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Crypts.Flyweights
{
    public class CryptInteractiveObject : BaseInteractiveObject
    {
        public CryptInteractiveObject(int id):base(id)
        {
            Inventory = new CryptInventoryData();
            ItemData = new CryptItemData();
        }
    }
}
