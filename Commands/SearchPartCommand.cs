using InventoryManager.ViewModels;
using System.Diagnostics;
using System.Linq;
using System.Windows;

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

                var matchingName = _homeViewModel.Parts.FirstOrDefault(x => x.Name.Contains(_homeViewModel.PartSearchBoxContents));
                var matchingID = _homeViewModel.Parts.FirstOrDefault(x => x.PartID.ToString().Contains(_homeViewModel.PartSearchBoxContents));
                if (matchingName == null && matchingID == null)
                {
                    MessageBox.Show($"No part names or IDs containing \"{_homeViewModel.PartSearchBoxContents}\" were found.", "No results found");
                    _homeViewModel.SelectedPart = null;
                    return;
                }

                if (matchingName != null)
                {
                    Debug.WriteLine($"Match found for \"{_homeViewModel.PartSearchBoxContents}\": {matchingName.Name}");
                    _homeViewModel.SelectedPart = matchingName;
                    return;
                }
                if(matchingID != null)
                {
                    Debug.WriteLine($"Match found for \"{_homeViewModel.PartSearchBoxContents}\": {matchingID.Name}");
                    _homeViewModel.SelectedPart = matchingID;
                    return;
                }
            }

            if (_addProductViewModel != null)
            {
                Debug.WriteLine($"Query is: {_addProductViewModel.SearchBoxContents}");
                if (string.IsNullOrWhiteSpace(_addProductViewModel.SearchBoxContents))
                {
                    Debug.WriteLine($"Query was empty.");
                    _addProductViewModel.SelectedPart = null;
                    return;
                }

                var matchingName = _addProductViewModel.Parts.FirstOrDefault(x => x.Name.Contains(_addProductViewModel.SearchBoxContents));
                var matchingID = _addProductViewModel.Parts.FirstOrDefault(x => x.PartID.ToString().Contains(_addProductViewModel.SearchBoxContents));
                if (matchingName == null && matchingID == null)
                {
                    MessageBox.Show($"No part names or IDs containing \"{_addProductViewModel.SearchBoxContents}\" were found.", "No results found");
                    _addProductViewModel.SelectedPart = null;
                    return;
                }

                if (matchingName != null)
                {
                    Debug.WriteLine($"Match found for \"{_addProductViewModel.SearchBoxContents}\": {matchingName.Name}");
                    _addProductViewModel.SelectedPart = matchingName;
                    return;
                }

                if(matchingID != null)
                {
                    Debug.WriteLine($"Match found for \"{_addProductViewModel.SearchBoxContents}\": {matchingID.Name}");
                    _addProductViewModel.SelectedPart = matchingID;
                    return;
                }
            }

            if(_modifyProductViewModel != null)
            {
                Debug.WriteLine($"Query is: {_modifyProductViewModel.SearchBoxContents}");
                if (string.IsNullOrWhiteSpace(_modifyProductViewModel.SearchBoxContents))
                {
                    Debug.WriteLine($"Query was empty.");
                    _modifyProductViewModel.SelectedPart = null;
                    return;
                }

                var matchingName = _modifyProductViewModel.Parts.FirstOrDefault(x => x.Name.Contains(_modifyProductViewModel.SearchBoxContents));
                var matchingID = _modifyProductViewModel.Parts.FirstOrDefault(x => x.PartID.ToString().Contains(_modifyProductViewModel.SearchBoxContents));
                if (matchingName == null && matchingID == null)
                {
                    MessageBox.Show($"No part names or IDs containing \"{_modifyProductViewModel.SearchBoxContents}\" were found.", "No results found");
                    _modifyProductViewModel.SelectedPart = null;
                    return;
                }

                if (matchingName != null)
                {
                    Debug.WriteLine($"Match found for \"{_modifyProductViewModel.SearchBoxContents}\": {matchingName.Name}");
                    _modifyProductViewModel.SelectedPart = matchingName;
                    return;
                }

                if(matchingID != null)
                {
                    Debug.WriteLine($"Match found for \"{_modifyProductViewModel.SearchBoxContents}\": {matchingID.Name}");
                    _modifyProductViewModel.SelectedPart = matchingID;
                    return;
                }
            }
        }
    }
}
