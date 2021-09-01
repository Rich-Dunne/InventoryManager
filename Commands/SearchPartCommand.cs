using InventoryManager.Interfaces;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace InventoryManager.Commands
{
    public class SearchPartCommand : CommandBase
    {
        private ISearchableViewModel _viewModel;

        public SearchPartCommand(ISearchableViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            Debug.WriteLine($"Query is: {_viewModel.PartSearchBoxContents}");
            if (string.IsNullOrWhiteSpace(_viewModel.PartSearchBoxContents))
            {
                Debug.WriteLine($"Query was empty.");
                _viewModel.SelectedPart = null;
                return;
            }

            var matchingName = _viewModel.Parts.FirstOrDefault(x => x.Name.Contains(_viewModel.PartSearchBoxContents));
            var matchingID = _viewModel.Parts.FirstOrDefault(x => x.PartID.ToString().Contains(_viewModel.PartSearchBoxContents));
            if (matchingName == null && matchingID == null)
            {
                MessageBox.Show($"No part names or IDs containing \"{_viewModel.PartSearchBoxContents}\" were found.", "No results found");
                _viewModel.SelectedPart = null;
                return;
            }

            if (matchingName != null)
            {
                Debug.WriteLine($"Match found for \"{_viewModel.PartSearchBoxContents}\": {matchingName.Name}");
                _viewModel.SelectedPart = matchingName;
                return;
            }
            if(matchingID != null)
            {
                Debug.WriteLine($"Match found for \"{_viewModel.PartSearchBoxContents}\": {matchingID.Name}");
                _viewModel.SelectedPart = matchingID;
                return;
            }
        }
    }
}
