using RPGBase.Flyweights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.FantasyWargaming.Flyweights
{
    public class FWInventoryData : InventoryData
    {
        public FWInventoryData()
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
