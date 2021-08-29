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

        public override void Execute(object param)
        {
            if(!IsInputValid())
            {
                return;
            }

            var newProduct = new Product(_viewModel.ProductID, _viewModel.ProductName, double.Parse(_viewModel.ProductPrice), int.Parse(_viewModel.ProductInventory), int.Parse(_viewModel.ProductMin), int.Parse(_viewModel.ProductMax));
            foreach(Part part in _viewModel.AssociatedParts)
            {
                newProduct.AddAssociatedPart(part);
            }
            
            Inventory.AddProduct(newProduct);
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

            return true;
        }
    }
}
