using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using InventoryManager.Commands;
using InventoryManager.Stores;

namespace InventoryManager.ViewModels
{
    public class AddProductViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        private ErrorsViewModel _errorsViewModel;
        #region Commands
        public ICommand NavigateHomeCommand { get; }
        public ICommand AddAssociatedPartCommand { get; internal set; }
        public ICommand DeleteAssociatedPartCommand { get; internal set; }
        public ICommand SaveNewProductCommand { get; internal set; }
        public ICommand SearchPartCommand { get; internal set; }
        #endregion

        #region DataGridSources
        public BindingList<Part> Parts { get; set; } = Inventory.Parts;
        public BindingList<Part> AssociatedParts { get; } = new BindingList<Part>();
        #endregion

        #region FormProperties
        public int ProductID { get; } = -1;
        private string _productName;
        public string ProductName 
        { 
            get => _productName;
            set
            {
                _productName = value;
                _errorsViewModel.ClearErrors(nameof(ProductName));
                if (string.IsNullOrWhiteSpace(_productName))
                {
                    _errorsViewModel.AddError(nameof(ProductName), "A product name is required");
                }
                OnPropertyChanged(nameof(ProductName));
            }
        }

        private int _productInventory;
        public int ProductInventory
        {
            get => _productInventory;

            set
            {
                _productInventory = value;
                ValidateInput();
                OnPropertyChanged(nameof(ProductInventory));
            }
        }

        private double _productPrice;
        public double ProductPrice
        {
            get => _productPrice;

            set
            {
                _productPrice = value;
                _errorsViewModel.ClearErrors(nameof(ProductPrice));
                if (_productPrice < 0)
                {
                    _errorsViewModel.AddError(nameof(ProductPrice), "Value must be at least 0");
                }
                OnPropertyChanged(nameof(ProductPrice));
            }
        }

        private int _productMin;
        public int ProductMin 
        {
            get => _productMin;

            set
            {
                _productMin = value;
                ValidateInput();
                OnPropertyChanged(nameof(ProductMin));
            }
        }

        private int _productMax;
        public int ProductMax
        {
            get => _productMax;

            set
            {
                _productMax = value;
                ValidateInput();
                OnPropertyChanged(nameof(ProductMax));
            }
        }
        #endregion

        private Part _selectedPart = null;
        public Part SelectedPart
        {
            get => _selectedPart;
            set
            {
                _selectedPart = value;
                OnPropertyChanged(nameof(SelectedPart));

                if (_selectedPart == null)
                {
                    PartSelected = false;
                }
                else
                {
                    PartSelected = true;
                }
            }
        }

        private bool _partSelected = false;
        public bool PartSelected
        {
            get => _partSelected;
            set
            {
                _partSelected = value;
                OnPropertyChanged(nameof(PartSelected));
            }
        }

        private Part _selectedAssociatedPart = null;
        public Part SelectedAssociatedPart
        {
            get => _selectedAssociatedPart;
            set
            {
                _selectedAssociatedPart = value;
                OnPropertyChanged(nameof(SelectedAssociatedPart));

                if (_selectedAssociatedPart == null)
                {
                    AssociatedPartSelected = false;
                }
                else
                {
                    AssociatedPartSelected = true;
                }
            }
        }
        private bool _associatedPartSelected = false;
        public bool AssociatedPartSelected
        {
            get => _associatedPartSelected;
            set
            {
                _associatedPartSelected = value;
                OnPropertyChanged(nameof(AssociatedPartSelected));
            }
        }

        public string SearchBoxContents { get; set; } = "";

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public bool HasErrors => _errorsViewModel.HasErrors;
        private bool _enableSave = false;
        public bool EnableSave
        {
            get => _enableSave;
            set
            {
                _enableSave = value;
                OnPropertyChanged(nameof(EnableSave));
            }
        }

        private ObservableCollection<Part> _tempAssociatedParts = new ObservableCollection<Part>();
        public ObservableCollection<Part> TempAssociatedParts
        {
            get => _tempAssociatedParts;
            set
            {
                _tempAssociatedParts = value;
                OnPropertyChanged(nameof(TempAssociatedParts));
            }
        }

        public AddProductViewModel(NavigationStore navigationStore)
        {
            NavigateHomeCommand = new NavigateHomeCommand(navigationStore);
            DeleteAssociatedPartCommand = new DeleteAssociatedPartCommand(this);
            AddAssociatedPartCommand = new AddAssociatedPartCommand(this);
            SaveNewProductCommand = new SaveNewProductCommand(this);
            SearchPartCommand = new SearchPartCommand(this);
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            _errorsViewModel.AddError(nameof(ProductName), "A product name is required.");
            ProductID = GetNewProductID();
        }

        private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            EnableSave = !HasErrors;
            ErrorsChanged?.Invoke(this, e);
        }

        private int GetNewProductID()
        {
            if (Inventory.DeletedProductIDs.Count > 0)
            {
                return Inventory.DeletedProductIDs.First();
            }
            else if (Inventory.Products.Count > 0)
            {
                return Inventory.Products.Last().ProductID + 1;
            }
            else
            {
                return 0;
            }
        }

        public IEnumerable GetErrors(string propertyName) => _errorsViewModel.GetErrors(propertyName);

        public void ValidateInput()
        {
            // When Min, Max, or Inventory are set, I need to clear all their errors and reassess
            _errorsViewModel.ClearErrors(nameof(ProductInventory));
            _errorsViewModel.ClearErrors(nameof(ProductMin));
            _errorsViewModel.ClearErrors(nameof(ProductMax));
            if (_productInventory > ProductMax || _productInventory < ProductMin)
            {
                _errorsViewModel.AddError(nameof(ProductInventory), "Inventory value must be greater than Min and less than Max");
            }
            if (_productInventory < 0)
            {
                _errorsViewModel.AddError(nameof(ProductInventory), "Inventory value must be at least 0");
            }
            if (_productMax < ProductMin)
            {
                _errorsViewModel.AddError(nameof(ProductMax), "Max value must be more than Min");
            }
            if (_productMax < ProductInventory)
            {
                _errorsViewModel.AddError(nameof(ProductMax), "Max value must be more than Inventory");
            }
            if (_productMin > ProductMax)
            {
                _errorsViewModel.AddError(nameof(ProductMin), "Min value must be less than Max");
            }
            if (_productMin > ProductInventory)
            {
                _errorsViewModel.AddError(nameof(ProductMin), "Min value must be less than Inventory");
            }
        }
    }
}
