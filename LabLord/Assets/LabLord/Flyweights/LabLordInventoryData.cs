using RPGBase.Flyweights;
using System;

namespace LabLord.Flyweights
{
    public class LabLordInventoryData : InventoryData
    {
        public LabLordInventoryData()
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