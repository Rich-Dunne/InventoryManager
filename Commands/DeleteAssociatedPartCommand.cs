using InventoryManager.ViewModels;

namespace InventoryManager.Commands
{
    public class DeleteAssociatedPartCommand : CommandBase
    {
        private AddProductViewModel _addProductViewModel;
        private ModifyProductViewModel _modifyProductViewModel;

        public DeleteAssociatedPartCommand(AddProductViewModel viewModel)
        {
            _addProductViewModel = viewModel;
        }

        public DeleteAssociatedPartCommand(ModifyProductViewModel viewModel)
        {
            _modifyProductViewModel = viewModel;
        }

        public override void Execute(object part)
        {
            if (_addProductViewModel != null && _addProductViewModel.SelectedItem != null)
            {
                _addProductViewModel.AssociatedParts.Remove((Part)_addProductViewModel.SelectedItem);
                return;
            }

            if (_modifyProductViewModel != null && _modifyProductViewModel.SelectedItem != null)
            {
                _modifyProductViewModel.TempAssociatedParts.Remove((Part)_modifyProductViewModel.SelectedItem);
                return;
            }
        }
    }
}
