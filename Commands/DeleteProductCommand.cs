using InventoryManager.Models;
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
            if(_viewModel.SelectedProduct == null)
            {
                return;
            }

            var productToRemove = _viewModel.SelectedProduct;
            if(productToRemove.AssociatedParts?.Count > 0)
            {
                MessageBox.Show($"Associated parts must be removed from \"{productToRemove.Name}\" before it can be deleted.");
                DeleteAssociatedParts(productToRemove);
                return;
            }

            DeleteProduct(productToRemove);
        }

        public void DeleteProduct(Product productToRemove)
        {
            var prompt = MessageBox.Show($"Are you sure you want to delete \"{productToRemove.Name}\"?", "Confirm product deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (prompt == MessageBoxResult.Yes)
            {
                Inventory.RemoveProduct(productToRemove.ProductID);
            }
        }

        public void DeleteAssociatedParts(Product productToRemove)
        {
            var associatedPartPrompt = MessageBox.Show($"Would you like to remove all associated parts from this product?", $"Remove associated parts", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (associatedPartPrompt == MessageBoxResult.Yes)
            {
                foreach (Part part in Inventory.Parts)
                {
                    productToRemove.RemoveAssociatedPart(part.PartID);
                }
                DeleteProduct(productToRemove);
            }
        }
    }
}
