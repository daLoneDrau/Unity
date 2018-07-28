using RPGBase.Flyweights;

namespace WoFM.Flyweights
{
    public class WoFMInteractiveObject : BaseInteractiveObject
    {
        public WoFMInteractiveObject(int id):base(id)
        {
            Inventory = new WoFMInventoryData();
            ItemData = new WoFMItemData();
        }
    }
}