using InventoryManager.Stores;
using InventoryManager.ViewModels;

namespace InventoryManager.Commands
{
    public class NavigateModifyProductCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly HomeViewModel _viewModel;

        public NavigateModifyProductCommand(NavigationStore navigationStore, HomeViewModel viewModel)
        {
            _navigationStore = navigationStore;
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            if(_viewModel.SelectedItem == null)
            {
                return;
            }
            _navigationStore.CurrentViewModel = new ModifyProductViewModel(_navigationStore, _viewModel);
        }
    }
}
