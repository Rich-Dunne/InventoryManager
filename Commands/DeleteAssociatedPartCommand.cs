using InventoryManager.ViewModels;

namespace InventoryManager.Commands
{
    public class DeleteAssociatedPartCommand : CommandBase
    {
        private AddProductViewModel _viewModel;

        public DeleteAssociatedPartCommand(AddProductViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object part)
        {
            if(_viewModel.SelectedItem == null)
            {
                return;
            }

            _viewModel.AssociatedParts.Remove((Part)_viewModel.SelectedItem);
        }
    }
}
