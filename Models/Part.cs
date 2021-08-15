using System.Linq;

namespace InventoryManager
{
    public abstract class Part
    {
        public int PartID { get; set; } = AssignUniqueID();
        public string Name { get; set; } = "";
        public double Price { get; set; } = -1;
        public int Inventory { get; set; } = -1;
        internal int Min { get; set; } = -1;
        internal int Max { get; set; } = -1;

        private static int AssignUniqueID()
        {
            if(InventoryManager.Inventory.Parts.Count == 0)
            {
                return 0;
            }

            if(InventoryManager.Inventory.DeletedPartIDs.Count > 0)
            {
                return InventoryManager.Inventory.DeletedPartIDs.Dequeue();
            }

            return InventoryManager.Inventory.Parts.Last().PartID + 1;
        }
    }
}
