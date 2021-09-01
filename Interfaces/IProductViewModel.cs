using InventoryManager.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace InventoryManager.Interfaces
{
    public interface IProductViewModel
    {
        ICommand NavigateHomeCommand { get; }
        int ProductID { get; set; }
        string ProductName { get; set; }
        string ProductInventory { get; set; }
        string ProductPrice { get; set; }
        string ProductMin { get; set; }
        string ProductMax { get; set; }
        void OnPropertyChanged(string propertyName);
        Part SelectedPart { get; set; }
        BindingList<Part> Parts { get; }
        BindingList<Part> AssociatedParts { get; }
        BindingList<Part> TempAssociatedParts { get; }
        string PartSearchBoxContents { get; }
        Product SelectedProduct { get; set; }
        BindingList<Product> Products { get; }
        string ProductSearchBoxContents { get; }
        bool PartSelected { get; set; }
        bool HasErrors { get; }
    }
}
