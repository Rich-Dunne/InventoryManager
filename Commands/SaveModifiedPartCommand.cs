using InventoryManager.Models;
using InventoryManager.ViewModels;
using System.Diagnostics;

namespace InventoryManager.Commands
{
    public class SaveModifiedPartCommand : CommandBase
    {
        private ModifyPartViewModel _viewModel;

        public SaveModifiedPartCommand(ModifyPartViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object part)
        {
            if(_viewModel.HasErrors)
            {
                return;
            }

            if(_viewModel.IsInHousePart)
            {
                Inventory.UpdatePart(_viewModel.PartID, new InHousePart(_viewModel.PartID, _viewModel.PartName, double.Parse(_viewModel.PartPrice), int.Parse(_viewModel.PartInventory), int.Parse(_viewModel.PartMin), int.Parse(_viewModel.PartMax), int.Parse(_viewModel.MachineID)));
            }
            else
            {
                Inventory.UpdatePart(_viewModel.PartID, new OutsourcedPart(_viewModel.PartID, _viewModel.PartName, double.Parse(_viewModel.PartPrice), int.Parse(_viewModel.PartInventory), int.Parse(_viewModel.PartMin), int.Parse(_viewModel.PartMax), _viewModel.CompanyName));
            }
            
            _viewModel.NavigateHomeCommand.Execute(null);
        }
    }
}
