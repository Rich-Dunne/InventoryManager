using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace InventoryManager.Models
{
    public class Product : INotifyPropertyChanged
    {
        private int _productID;
        public int ProductID 
        { 
            get => _productID;
            set
            {
                _productID = value;
                OnPropertyChanged(nameof(ProductID));
            }
        }
        private string _name;
        public string Name 
        { 
            get => _name; 
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private double _price;
        public double Price 
        { 
            get => _price; 
            set
            {
                _price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        private int _inventory;
        public int Inventory
        {
            get => _inventory;
            set
            {
                _inventory = value;
                OnPropertyChanged(nameof(Inventory));
            }
        }

        private int _min;
        public int Min 
        { 
            get => _min; 
            set
            {
                _min = value;
                OnPropertyChanged(nameof(Min));
            }
        }

        private int _max;
        public int Max 
        {
            get => _max;
            set
            {
                _max = value;
                OnPropertyChanged(nameof(Max));
            }
        }

        private ObservableCollection<Part> _associatedParts;
        public ObservableCollection<Part> AssociatedParts 
        { 
            get => _associatedParts;
            set
            {
                _associatedParts = value;
                OnPropertyChanged(nameof(AssociatedParts));
            }
        }

        internal Product(string name, double price, int inventory, int min, int max, ObservableCollection<Part> associatedParts)
        {
            _productID = AssignUniqueID();
            Name = name;
            Price = price;
            Inventory = inventory;
            Min = min;
            Max = max;
            AssociatedParts = associatedParts;
        }

        internal Product(int productID, string name, double price, int inventory, int min, int max, ObservableCollection<Part> associatedParts)
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
