using InventoryManager.Models;
using InventoryManager.ViewModels;
using System.Diagnostics;
using System.Windows;

namespace InventoryManager.Commands
{
    public class SaveModifiedProductCommand : CommandBase
    {
        private ModifyProductViewModel _viewModel;

        public SaveModifiedProductCommand(ModifyProductViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object param)
        {
            if(!IsInputValid())
            {
                return;
            }

            var modifiedProduct = new Product(_viewModel.ProductID, _viewModel.ProductName, double.Parse(_viewModel.ProductPrice), int.Parse(_viewModel.ProductInventory), int.Parse(_viewModel.ProductMin), int.Parse(_viewModel.ProductMax));
            foreach (Part part in _viewModel.AssociatedParts)
            {
                modifiedProduct.AddAssociatedPart(part);
            }

            Inventory.UpdateProduct(_viewModel.ProductID, modifiedProduct);
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
