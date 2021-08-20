using InventoryManager.Stores;
using InventoryManager.ViewModels;

namespace InventoryManager.Commands
{
    public class NavigateAddPartCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;

        public NavigateAddPartCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            _navigationStore.CurrentViewModel = new AddPartViewModel(_navigationStore);
        }
    }
}
