using InventoryManager.ViewModels;
using System.Linq;

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

        public override void Execute(object param)
        {
            if(_addProductViewModel != null)
            {
                var partToRemove = _addProductViewModel.AssociatedParts.FirstOrDefault(x => x.PartID == _addProductViewModel.SelectedAssociatedPart.PartID);
                _addProductViewModel.AssociatedParts.Remove(partToRemove);
                return;
            }

            if (_modifyProductViewModel != null)
            {
                _modifyProductViewModel.AssociatedParts.Remove(_modifyProductViewModel.SelectedAssociatedPart);
                return;
            }
        }
    }
}
