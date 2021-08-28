using InventoryManager.Models;
using InventoryManager.ViewModels;
using System.Diagnostics;
using System.Windows;

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
                Inventory.UpdatePart(_viewModel.PartID, new InHousePart(_viewModel.PartID, _viewModel.PartName, _viewModel.PartPrice, _viewModel.PartInventory, _viewModel.PartMin, _viewModel.PartMax, _viewModel.MachineID));
            }
            else
            {
                Inventory.UpdatePart(_viewModel.PartID, new OutsourcedPart(_viewModel.PartID, _viewModel.PartName, _viewModel.PartPrice, _viewModel.PartInventory, _viewModel.PartMin, _viewModel.PartMax, _viewModel.CompanyName));
            }
            
            _viewModel.NavigateHomeCommand.Execute(null);
        }

        //private bool IsInputValid()
        //{
        //    if (string.IsNullOrWhiteSpace(_viewModel.PartName))
        //    {
        //        MessageBox.Show($"A product name is required.");
        //        return false;
        //    }
        //    if (_viewModel.HasErrors)
        //    {
        //        MessageBox.Show($"Please fix the errors before attempting to add a new product.");
        //        return false;
        //    }

        //    return true;
        //}
    }
}
