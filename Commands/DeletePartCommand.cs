using InventoryManager.Models;
using InventoryManager.ViewModels;
using System.Linq;
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

            var partToRemove = _viewModel.SelectedPart;
            

            var warningPrompt = MessageBox.Show($"Are you sure you want to delete \"{partToRemove.Name}\"?", "Confirm part deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (warningPrompt == MessageBoxResult.Yes)
            {
                bool partBelongsToAnyProduct = Inventory.Products.Any(x => x.AssociatedParts.Any(y => y.PartID == partToRemove.PartID));
                if (partBelongsToAnyProduct)
                {
                    DeletePartFromAllProducts(partToRemove);
                }
                Inventory.DeletePart(partToRemove);
            }
        }

        private void DeletePartFromAllProducts(Part partToRemove)
        {
            var associatedPartPrompt = MessageBox.Show($"Part \"{partToRemove.Name}\" is associated with one or more products.  Would you like to remove this part from all products?", $"Remove part from all products", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (associatedPartPrompt == MessageBoxResult.Yes)
            {
                foreach (Product product in Inventory.Products)
                {
                    if (product.AssociatedParts.Any(x => x.PartID == partToRemove.PartID))
                    {
                        product.RemoveAssociatedPart(partToRemove.PartID);
                    }
                }
            }
        }
    }
}
