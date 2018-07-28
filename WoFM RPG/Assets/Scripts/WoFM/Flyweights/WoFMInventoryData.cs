using RPGBase.Flyweights;
using System;

namespace WoFM.Flyweights
{
    public class WoFMInventoryData : InventoryData
    {
        public WoFMInventoryData()
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