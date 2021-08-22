using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace InventoryManager.Models
{
    public class Product : INotifyPropertyChanged
    {
        private int _productID;
        public int ProductID { get => _productID; set => OnPropertyChanged("ProductID"); }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Inventory { get; set; }
        internal int Min { get; set; }
        internal int Max { get; set; }
        internal BindingList<Part> AssociatedParts { get; }

        internal Product(string name, double price, int inventory, int min, int max, BindingList<Part> associatedParts)
        {
            _productID = AssignUniqueID();
            Name = name;
            Price = price;
            Inventory = inventory;
            Min = min;
            Max = max;
            AssociatedParts = associatedParts;
        }

        internal Product(int productID, string name, double price, int inventory, int min, int max, BindingList<Part> associatedParts)
        {
            _productID = productID;
            Name = name;
            Price = price;
            Inventory = inventory;
            Min = min;
            Max = max;
            AssociatedParts = associatedParts;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
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

        }

        internal bool RemoveAssociatedPart(int partID)
        {
            return false;
        }

        internal Part LookupAssociatedPart(int partID)
        {
            return null;
        }
    }
}
