using System.ComponentModel;
using System.Windows.Input;
using InventoryManager.Commands;
using InventoryManager.Stores;

namespace InventoryManager.ViewModels
{
    public class AddPartViewModel : BaseViewModel
    {
        #region Commands
        public ICommand NavigateHomeCommand { get; }
        #endregion

        #region Properties
        public BindingList<Part> Parts { get; set; } = Inventory.Parts;
        #endregion

        public AddPartViewModel(NavigationStore navigationStore)
        {
            NavigateHomeCommand = new NavigateHomeCommand(navigationStore);
        }
    }
}
