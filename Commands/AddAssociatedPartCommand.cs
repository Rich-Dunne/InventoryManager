using InventoryManager.ViewModels;
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

        public override void Execute(object param)
        {
            Part selectedPart;
            if(_addProductViewModel != null)
            {
                selectedPart = _addProductViewModel.SelectedPart;
                if (selectedPart == null)
                {
                    return;
                }

                if (_addProductViewModel.AssociatedParts.Any(x => x.PartID == selectedPart.PartID))
                {
                    return;
                }

                _addProductViewModel.AssociatedParts.Add(selectedPart);
                Debug.WriteLine($"Added part \"{selectedPart.Name}\" to product.");
            }

            if (_modifyProductViewModel != null)
            {
                selectedPart = _modifyProductViewModel.SelectedPart;
                if(selectedPart == null)
                {
                    return;
                }

                if(_modifyProductViewModel.AssociatedParts.Any(x => x.PartID == selectedPart.PartID))
                {
                    return;
                }

                _modifyProductViewModel.AssociatedParts.Add(selectedPart);
                _modifyProductViewModel.TempAssociatedParts.Add(selectedPart);

                Debug.WriteLine($"Added part \"{selectedPart.Name}\" to product.");
            }
        }
    }
}
