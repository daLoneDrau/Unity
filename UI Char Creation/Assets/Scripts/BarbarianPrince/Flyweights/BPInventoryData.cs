using RPGBase.Flyweights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.BarbarianPrince.Flyweights
{
    public class BPInventoryData : InventoryData
    {
        public BPInventoryData()
        {
            Slots = new InventorySlot[16];
            for (int i = 0; i < Slots.Length; i++)
            {
                Slots[i] = new InventorySlot();
            }
        }
        public override void PutInFrontOfPlayer(BaseInteractiveObject itemIO, bool doNotApplyPhysics)
        {
            throw new NotImplementedException();
        }
    }
}
