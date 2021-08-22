using InventoryManager.Models;
using InventoryManager.ViewModels;
using System.Windows;

namespace InventoryManager.Commands
{
    public class SaveNewProductCommand : CommandBase
    {
        private AddProductViewModel _viewModel;

        public SaveNewProductCommand(AddProductViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object part)
        {
            if(!IsInputValid())
            {
                return;
            }

            Inventory.AddProduct(new Product(_viewModel.ProductID, _viewModel.ProductName, _viewModel.ProductPrice, _viewModel.ProductInventory, _viewModel.ProductMin, _viewModel.ProductMax, _viewModel.AssociatedParts));
            _viewModel.NavigateHomeCommand.Execute(null);
        }

        private bool IsInputValid()
        {
            if (string.IsNullOrWhiteSpace(_viewModel.ProductName))
            {
                MessageBox.Show($"A product name is required.");
                return false;
            }
            if (_viewModel.HasErrors)
            {
                MessageBox.Show($"Please fix the errors before attempting to add a new product.");
                return false;
            }
            if (_viewModel.AssociatedParts.Count < 1)
            {
                MessageBox.Show($"A product requires at least one associated part.");
                return false;
            }

            return true;
        }
    }
}
