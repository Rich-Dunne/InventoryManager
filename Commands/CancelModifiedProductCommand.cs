using InventoryManager.ViewModels;
using System.Diagnostics;
using System.Linq;

namespace InventoryManager.Commands
{
    public class CancelModifiedProductCommand : CommandBase
    {
        private ModifyProductViewModel _viewModel;

        public CancelModifiedProductCommand(ModifyProductViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object param)
        {
            var matchingParts = _viewModel.ProductBeingModified.AssociatedParts.Where(x => _viewModel.TempAssociatedParts.Any(y => y.PartID == x.PartID));
            Debug.WriteLine($"Matching parts: {matchingParts.Count()}");
            foreach(Part part in matchingParts.ToList())
            {
                _viewModel.ProductBeingModified.RemoveAssociatedPart(part.PartID);
            }

            _viewModel.NavigateHomeCommand.Execute(null);
        }
    }
}
