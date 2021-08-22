using InventoryManager.ViewModels;

namespace InventoryManager.Commands
{
    public class AddAssociatedPartCommand : CommandBase
    {
        private AddProductViewModel _viewModel;

        public AddAssociatedPartCommand(AddProductViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object part)
        {
            if(_viewModel.SelectedItem == null || _viewModel.AssociatedParts.Contains((Part)_viewModel.SelectedItem))
            {
                return;
            }

            _viewModel.AssociatedParts.Add((Part)_viewModel.SelectedItem);
        }
    }
}
