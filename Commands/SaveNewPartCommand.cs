using InventoryManager.Models;
using InventoryManager.ViewModels;
using System.Windows;

namespace InventoryManager.Commands
{
    public class SaveNewPartCommand : CommandBase
    {
        private AddPartViewModel _viewModel;

        public SaveNewPartCommand(AddPartViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object part)
        {
            if(!IsInputValid())
            {
                return;
            }

            if(_viewModel.IsInHousePart)
            {
                Inventory.AddPart(new InHousePart(_viewModel.PartID, _viewModel.PartName, double.Parse(_viewModel.PartPrice), int.Parse(_viewModel.PartInventory), int.Parse(_viewModel.PartMin), int.Parse(_viewModel.PartMax), int.Parse(_viewModel.MachineID)));
            }
            else
            {
                Inventory.AddPart(new OutsourcedPart(_viewModel.PartID, _viewModel.PartName, double.Parse(_viewModel.PartPrice), int.Parse(_viewModel.PartInventory), int.Parse(_viewModel.PartMin), int.Parse(_viewModel.PartMax), _viewModel.CompanyName));
            }
            _viewModel.NavigateHomeCommand.Execute(null);
        }

        private bool IsInputValid()
        {
            if (string.IsNullOrWhiteSpace(_viewModel.PartName))
            {
                MessageBox.Show($"A product name is required.");
                return false;
            }
            if (_viewModel.HasErrors)
            {
                MessageBox.Show($"Please fix the errors before attempting to add a new product.");
                return false;
            }

            return true;
        }
    }
}
