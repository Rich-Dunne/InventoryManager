using InventoryManager.ViewModels;
using System.Diagnostics;
using System.Linq;

namespace InventoryManager.Commands
{
    public class SearchProductCommand : CommandBase
    {
        private HomeViewModel _viewModel;

        public SearchProductCommand(HomeViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            Debug.WriteLine($"Query is: {_viewModel.ProductSearchBoxContents}");
            if(string.IsNullOrWhiteSpace(_viewModel.ProductSearchBoxContents))
            {
                Debug.WriteLine($"Query was empty.");
                _viewModel.SelectedItem = null;
                return;
            }
            var match = _viewModel.Products.FirstOrDefault(x => x.Name.Contains(_viewModel.ProductSearchBoxContents));
            if(match == null)
            {
                Debug.WriteLine($"No matches found.");
                return;
            }

            Debug.WriteLine($"Match found for \"{_viewModel.ProductSearchBoxContents}\": {match.Name}");
            _viewModel.SelectedItem = match;
        }
    }
}
