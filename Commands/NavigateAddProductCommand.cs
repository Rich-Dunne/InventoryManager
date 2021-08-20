using InventoryManager.Stores;
using InventoryManager.ViewModels;

namespace InventoryManager.Commands
{
    public class NavigateAddProductCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;

        public NavigateAddProductCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            _navigationStore.CurrentViewModel = new AddProductViewModel(_navigationStore);
        }
    }
}
