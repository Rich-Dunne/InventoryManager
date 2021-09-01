using System.Windows.Input;

namespace InventoryManager.Interfaces
{
    public interface IPartViewModel
    {
        ICommand NavigateHomeCommand { get; }
        int PartID { get; set; }
        string PartName { get; set; }
        string PartInventory { get; set; }
        string PartPrice { get; set; }
        string PartMin { get; set; }
        string PartMax { get; set; }
        string CompanyName { get; set; }
        string MachineID { get; set; }
        bool IsInHousePart { get; set; }
        bool IsOutsourcedPart { get; set; }
        void OnPropertyChanged(string propertyName);
        bool HasErrors { get; }
    }
}
