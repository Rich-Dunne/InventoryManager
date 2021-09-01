using InventoryManager.Models;
using System.ComponentModel;

namespace InventoryManager.Interfaces
{
    public interface ISearchableViewModel
    {
        Part SelectedPart { get; set; }
        BindingList<Part> Parts { get; }
        string PartSearchBoxContents { get; }
        Product SelectedProduct { get; set; }
        BindingList<Product> Products { get; }
        string ProductSearchBoxContents { get; }
    }
}
