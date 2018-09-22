using RPGBase.Flyweights;
using UnityEngine;

namespace WoFM.Flyweights
{
    public class WoFMInteractiveObject : BaseInteractiveObject
    {
        public WoFMInteractiveObject(int id):base(id)
        {
            Inventory = new WoFMInventoryData();
            ItemData = new WoFMItemData();
        }
        /// <summary>
        /// the last position the IO was at.  During moves, the IO is still considered to be at this position for lighting and other purposes.
        /// </summary>
        public Vector2 LastPositionHeld { get; set; }
    }
}