using InventoryManager.Interfaces;
using InventoryManager.Models;
using InventoryManager.ViewModels;
using System.Diagnostics;

namespace InventoryManager.Commands
{
    public class SavePartCommand : CommandBase
    {
        private IPartViewModel _viewModel;

        public SavePartCommand(IPartViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object part)
        {
            if(_viewModel.HasErrors)
            {
                return;
            }

            if(_viewModel is AddPartViewModel)
            {
                AddNewPart();
            }
            else if (_viewModel is ModifyPartViewModel)
            {
                UpdatePart();
            }
            _viewModel.NavigateHomeCommand.Execute(null);
        }

        private void AddNewPart()
        {
            if (_viewModel.IsInHousePart)
            {
                Inventory.AddPart(new InHousePart(_viewModel.PartID, _viewModel.PartName, double.Parse(_viewModel.PartPrice), int.Parse(_viewModel.PartInventory), int.Parse(_viewModel.PartMin), int.Parse(_viewModel.PartMax), int.Parse(_viewModel.MachineID)));
            }
            else
            {
                Inventory.AddPart(new OutsourcedPart(_viewModel.PartID, _viewModel.PartName, double.Parse(_viewModel.PartPrice), int.Parse(_viewModel.PartInventory), int.Parse(_viewModel.PartMin), int.Parse(_viewModel.PartMax), _viewModel.CompanyName));
            }
        }

        private void UpdatePart()
        {
            if (_viewModel.IsInHousePart)
            {
                Inventory.UpdatePart(_viewModel.PartID, new InHousePart(_viewModel.PartID, _viewModel.PartName, double.Parse(_viewModel.PartPrice), int.Parse(_viewModel.PartInventory), int.Parse(_viewModel.PartMin), int.Parse(_viewModel.PartMax), int.Parse(_viewModel.MachineID)));
            }
            else
            {
                Inventory.UpdatePart(_viewModel.PartID, new OutsourcedPart(_viewModel.PartID, _viewModel.PartName, double.Parse(_viewModel.PartPrice), int.Parse(_viewModel.PartInventory), int.Parse(_viewModel.PartMin), int.Parse(_viewModel.PartMax), _viewModel.CompanyName));
            }
        }
    }
}
