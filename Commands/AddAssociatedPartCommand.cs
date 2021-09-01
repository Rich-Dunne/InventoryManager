using InventoryManager.Interfaces;
using InventoryManager.ViewModels;
using System.Diagnostics;
using System.Linq;

namespace InventoryManager.Commands
{
    public class AddAssociatedPartCommand : CommandBase
    {
        private IProductViewModel _viewModel;

        public AddAssociatedPartCommand(IProductViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object param)
        {
            Part selectedPart = _viewModel.SelectedPart;
            if (selectedPart == null)
            {
                return;
            }

            if (_viewModel.AssociatedParts.Any(x => x.PartID == selectedPart.PartID))
            {
                return;
            }

            _viewModel.AssociatedParts.Add(selectedPart);
            _viewModel.OnPropertyChanged(nameof(_viewModel.AssociatedParts));
            _viewModel.PartSelected = false;
            if (_viewModel is ModifyProductViewModel)
            {
                _viewModel.TempAssociatedParts.Add(selectedPart);
            }

            Debug.WriteLine($"Added part \"{selectedPart.Name}\" to product.");
        }
    }
}
