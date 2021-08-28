using InventoryManager.ViewModels;
using System.Windows;

namespace InventoryManager.Commands
{
    public class DeletePartCommand : CommandBase
    {
        private readonly HomeViewModel _viewModel;
        public DeletePartCommand(HomeViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            if(_viewModel.SelectedPart == null)
            {
                return;
            }

            var partToRemove = (Part)_viewModel.SelectedPart;
          
            var prompt = MessageBox.Show($"Are you sure you want to delete \"{partToRemove.Name}\"?", "Confirm part deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (prompt == MessageBoxResult.Yes)
            {
                Inventory.DeletePart(partToRemove);
            }
        }
    }
}
