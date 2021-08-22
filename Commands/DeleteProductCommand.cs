using InventoryManager.Models;
using InventoryManager.Stores;
using InventoryManager.ViewModels;
using System.Windows;

namespace InventoryManager.Commands
{
    public class DeleteProductCommand : CommandBase
    {
        private readonly HomeViewModel _viewModel;
        public DeleteProductCommand(HomeViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            if(_viewModel.SelectedItem == null)
            {
                return;
            }

            var productToRemove = (Product)_viewModel.SelectedItem;
            if(productToRemove.AssociatedParts?.Count > 0)
            {
                MessageBox.Show($"Associated parts must be removed from \"{productToRemove.Name}\" before it can be deleted.");
                return;
            }
          
            var prompt = MessageBox.Show($"Are you sure you want to delete \"{productToRemove.Name}\"?", "Confirm product deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (prompt == MessageBoxResult.Yes)
            {
                Inventory.RemoveProduct(productToRemove.ProductID);
            }
        }
    }
}
