using InventoryManager.ViewModels;
using System.Diagnostics;
using System.Linq;

namespace InventoryManager.Commands
{
    public class SearchPartCommand : CommandBase
    {
        private HomeViewModel _homeViewModel;
        private AddProductViewModel _addProductViewModel;
        private ModifyProductViewModel _modifyProductViewModel;

        public SearchPartCommand(HomeViewModel viewModel)
        {
            _homeViewModel = viewModel;
        }

        public SearchPartCommand(AddProductViewModel viewModel)
        {
            _addProductViewModel = viewModel;
        }

        public SearchPartCommand(ModifyProductViewModel viewModel)
        {
            _modifyProductViewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            if (_homeViewModel != null)
            {
                Debug.WriteLine($"Query is: {_homeViewModel.PartSearchBoxContents}");
                if (string.IsNullOrWhiteSpace(_homeViewModel.PartSearchBoxContents))
                {
                    Debug.WriteLine($"Query was empty.");
                    _homeViewModel.SelectedPart = null;
                    return;
                }
                var match = _homeViewModel.Parts.FirstOrDefault(x => x.Name.Contains(_homeViewModel.PartSearchBoxContents));
                if (match == null)
                {
                    Debug.WriteLine($"No matches found.");
                    return;
                }

                Debug.WriteLine($"Match found for \"{_homeViewModel.PartSearchBoxContents}\": {match.Name}");
                _homeViewModel.SelectedPart = match;
                return;
            }

            if (_addProductViewModel != null)
            {
                Debug.WriteLine($"Query is: {_addProductViewModel.SearchBoxContents}");
                if (string.IsNullOrWhiteSpace(_addProductViewModel.SearchBoxContents))
                {
                    Debug.WriteLine($"Query was empty.");
                    _addProductViewModel.SelectedItem = null;
                    return;
                }
                var match = _addProductViewModel.Parts.FirstOrDefault(x => x.Name.Contains(_addProductViewModel.SearchBoxContents));
                if (match == null)
                {
                    Debug.WriteLine($"No matches found.");
                    return;
                }

                Debug.WriteLine($"Match found for \"{_addProductViewModel.SearchBoxContents}\": {match.Name}");
                _addProductViewModel.SelectedItem = match;
                return;
            }

            if(_modifyProductViewModel != null)
            {
                Debug.WriteLine($"Query is: {_modifyProductViewModel.SearchBoxContents}");
                if (string.IsNullOrWhiteSpace(_modifyProductViewModel.SearchBoxContents))
                {
                    Debug.WriteLine($"Query was empty.");
                    _modifyProductViewModel.SelectedItem = null;
                    return;
                }
                var match = _modifyProductViewModel.Parts.FirstOrDefault(x => x.Name.Contains(_modifyProductViewModel.SearchBoxContents));
                if (match == null)
                {
                    Debug.WriteLine($"No matches found.");
                    return;
                }

                Debug.WriteLine($"Match found for \"{_modifyProductViewModel.SearchBoxContents}\": {match.Name}");
                _modifyProductViewModel.SelectedItem = match;
                return;
            }
        }
    }
}
