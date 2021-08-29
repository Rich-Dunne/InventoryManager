using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace InventoryManager.Models
{
    public class Product
    {
        public int ProductID { get; set; } = AssignUniqueID();
        public string Name { get; set; }
        public double Price { get; set; }
        public int Inventory { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public BindingList<Part> AssociatedParts { get; set; }

        internal Product(string name, double price, int inventory, int min, int max, BindingList<Part> associatedParts)
        {
            Name = name;
            Price = price;
            Inventory = inventory;
            Min = min;
            Max = max;
            AssociatedParts = associatedParts;
        }

        internal Product(int productID, string name, double price, int inventory, int min, int max)
        {
            ProductID = productID;
            Name = name;
            Price = price;
            Inventory = inventory;
            Min = min;
            Max = max;
            AssociatedParts = new BindingList<Part>();
        }

        private static int AssignUniqueID()
        {          
            if (InventoryManager.Inventory.Products.Count == 0)
            {
                return 0;
            }

            if (InventoryManager.Inventory.DeletedProductIDs.Count > 0)
            {
                return InventoryManager.Inventory.DeletedProductIDs.Dequeue();
            }

            return InventoryManager.Inventory.Products.Last().ProductID + 1;
        }

        internal void AddAssociatedPart(Part part)
        {
            if(part == null)
            {
                return;
            }

            if (!AssociatedParts.Any(x => x.PartID == part.PartID))
            {
                AssociatedParts.Add(part);
            }
        }

        internal bool RemoveAssociatedPart(int partID)
        {
            var partToRemove = AssociatedParts.FirstOrDefault(x => x.PartID == partID);
            if(partToRemove == null)
            {
                return false;
            }

            AssociatedParts.Remove(partToRemove);
            return true;
        }

        internal Part LookupAssociatedPart(int partID) => AssociatedParts.FirstOrDefault(x => x.PartID == partID);
    }
}
