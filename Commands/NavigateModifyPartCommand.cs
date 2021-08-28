using InventoryManager.Stores;
using InventoryManager.ViewModels;
using System.Diagnostics;

namespace InventoryManager.Commands
{
    public class NavigateModifyPartCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly HomeViewModel _viewModel;

        public NavigateModifyPartCommand(NavigationStore navigationStore, HomeViewModel viewModel)
        {
            _navigationStore = navigationStore;
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            if(_viewModel.SelectedPart == null)
            {
                return;
            }

            _navigationStore.CurrentViewModel = new ModifyPartViewModel(_navigationStore, _viewModel);
        }
    }
}
