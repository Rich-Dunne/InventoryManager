using InventoryManager.ViewModels;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace InventoryManager.Commands
{
    public class AddAssociatedPartCommand : CommandBase
    {
        private AddProductViewModel _addProductViewModel;
        private ModifyProductViewModel _modifyProductViewModel;

        public AddAssociatedPartCommand(AddProductViewModel viewModel)
        {
            _addProductViewModel = viewModel;
        }

        public AddAssociatedPartCommand(ModifyProductViewModel viewModel)
        {
            _modifyProductViewModel = viewModel;
        }

        public override void Execute(object part)
        {
            if (_addProductViewModel?.SelectedItem != null && !_addProductViewModel.AssociatedParts.Contains((Part)_addProductViewModel.SelectedItem))
            {
                _addProductViewModel.AssociatedParts.Add((Part)_addProductViewModel.SelectedItem);
                return;
            }

            Part selectedPart = (Part)_modifyProductViewModel.SelectedItem;
            if (_modifyProductViewModel?.SelectedItem != null && !_modifyProductViewModel.TempAssociatedParts.Any(x => x.PartID == selectedPart.PartID))
            {
                _modifyProductViewModel.TempAssociatedParts.Add((Part)_modifyProductViewModel.SelectedItem);
                return;
            }
        }
    }
}
