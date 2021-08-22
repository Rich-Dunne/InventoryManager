using InventoryManager.ViewModels;
using System.Diagnostics;
using System.Linq;

namespace InventoryManager.Commands
{
    public class SearchPartCommand : CommandBase
    {
        private AddProductViewModel _viewModel;

        public SearchPartCommand(AddProductViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            Debug.WriteLine($"Query is: {_viewModel.SearchBoxContents}");
            if(string.IsNullOrWhiteSpace(_viewModel.SearchBoxContents))
            {
                Debug.WriteLine($"Query was empty.");
                _viewModel.SelectedItem = null;
                return;
            }
            var match = _viewModel.Parts.FirstOrDefault(x => x.Name.Contains(_viewModel.SearchBoxContents));
            if(match == null)
            {
                Debug.WriteLine($"No matches found.");
                return;
            }

            Debug.WriteLine($"Match found for \"{_viewModel.SearchBoxContents}\": {match.Name}");
            _viewModel.SelectedItem = match;
        }
    }
}
