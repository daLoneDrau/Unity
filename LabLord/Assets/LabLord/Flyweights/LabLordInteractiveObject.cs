using RPGBase.Flyweights;

namespace LabLord.Flyweights
{
    public class LabLordInteractiveObject : BaseInteractiveObject
    {
        public LabLordInteractiveObject(int id):base(id)
        {
            Inventory = new LabLordInventoryData();
            ItemData = new LabLordItemData();
        }
    }
}