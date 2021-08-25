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

        public override void Execute(object part)
        {
            if(!IsInputValid())
            {
                return;
            }

            foreach (Part associatedPart in _viewModel.TempAssociatedParts)
            {
                _viewModel.AssociatedParts.Add(associatedPart);
            }
            Inventory.UpdateProduct(_viewModel.ProductID, new Product(_viewModel.ProductID, _viewModel.ProductName, _viewModel.ProductPrice, _viewModel.ProductInventory, _viewModel.ProductMin, _viewModel.ProductMax, _viewModel.AssociatedParts));
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
            if (_viewModel.TempAssociatedParts.Count < 1)
            {
                MessageBox.Show($"A product requires at least one associated part.");
                return false;
            }

            return true;
        }
    }
}
