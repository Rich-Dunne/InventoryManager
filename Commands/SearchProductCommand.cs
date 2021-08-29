using InventoryManager.ViewModels;
using System.Diagnostics;
using System.Linq;
using System.Windows;

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
                _viewModel.SelectedProduct = null;
                return;
            }
            var matchingName = _viewModel.Products.FirstOrDefault(x => x.Name.Contains(_viewModel.ProductSearchBoxContents));
            var matchingID = _viewModel.Products.FirstOrDefault(x => x.ProductID.ToString().Contains(_viewModel.ProductSearchBoxContents));
            if (matchingName == null && matchingID == null)
            {
                MessageBox.Show($"No product names or IDs containing \"{_viewModel.ProductSearchBoxContents}\" were found.", "No results found");
                _viewModel.SelectedProduct = null;
                return;
            }

            if (matchingName != null)
            {
                Debug.WriteLine($"Match found for \"{_viewModel.ProductSearchBoxContents}\": {matchingName.Name}");
                _viewModel.SelectedProduct = matchingName;
                return;
            }

            if (matchingID != null)
            {
                Debug.WriteLine($"Match found for \"{_viewModel.ProductSearchBoxContents}\": {matchingID.Name}");
                _viewModel.SelectedProduct = matchingID;
                return;
            }
        }
    }
}
