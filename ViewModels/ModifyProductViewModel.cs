using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using InventoryManager.Commands;
using InventoryManager.Models;
using InventoryManager.Stores;

namespace InventoryManager.ViewModels
{
    public class ModifyProductViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        private HomeViewModel _homeViewModel;
        private ErrorsViewModel _errorsViewModel;
        #region Commands
        public ICommand NavigateHomeCommand { get; }
        public ICommand AddAssociatedPartCommand { get; internal set; }
        public ICommand DeleteAssociatedPartCommand { get; internal set; }
        public ICommand SaveModifiedProductCommand { get; internal set; }
        public ICommand SearchPartCommand { get; internal set; }
        public ICommand CancelModifiedProductCommand { get; internal set; }
        #endregion

        #region DataGridSources
        public BindingList<Part> Parts { get; set; } = Inventory.Parts;
        public BindingList<Part> AssociatedParts { get; set; } = new BindingList<Part>();
        #endregion

        #region FormProperties
        public int ProductID { get; set; } = -1;
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

        private int? _productInventory;
        public string ProductInventory
        {
            get => _productInventory.ToString();

            set
            {
                if (int.TryParse(value, out var number))
                {
                    _productInventory = number;
                }
                else
                {
                    _productInventory = null;
                }
                ValidateInput();
                OnPropertyChanged(nameof(ProductInventory));
            }
        }

        private double? _productPrice;
        public string ProductPrice
        {
            get => _productPrice.ToString();

            set
            {
                if (double.TryParse(value, out var number))
                {
                    _productPrice = number;
                }
                else
                {
                    _productPrice = null;
                }
                ValidateInput();
                OnPropertyChanged(nameof(ProductPrice));
            }
        }

        private int? _productMin;
        public string ProductMin
        {
            get => _productMin.ToString();

            set
            {
                if (int.TryParse(value, out var number))
                {
                    _productMin = number;
                }
                else
                {
                    _productMin = null;
                }
                ValidateInput();
                OnPropertyChanged(nameof(ProductMin));
            }
        }

        private int? _productMax;
        public string ProductMax
        {
            get => _productMax.ToString();

            set
            {
                if (int.TryParse(value, out var number))
                {
                    _productMax = number;
                }
                else
                {
                    _productMax = null;
                }
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
                else if (_selectedPart != null && AssociatedParts.Any(x => x.PartID == _selectedPart.PartID))
                {
                    PartSelected = false;
                    SelectedAssociatedPart = null;
                }
                else
                {
                    PartSelected = true;
                    SelectedAssociatedPart = null;
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
                    SelectedPart = null;
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
        private Product _productBeingModified;
        public Product ProductBeingModified
        {
            get => _productBeingModified;
            set
            {
                _productBeingModified = value;
                OnPropertyChanged(nameof(ProductBeingModified));
            }
        }

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

        private ObservableCollection<Part> _tempDeletedParts = new ObservableCollection<Part>();
        public ObservableCollection<Part> TempDeletedParts
        {
            get => _tempDeletedParts;
            set
            {
                _tempDeletedParts = value;
                OnPropertyChanged(nameof(TempDeletedParts));
            }
        }

        public ModifyProductViewModel(NavigationStore navigationStore, HomeViewModel viewModel)
        {
            _homeViewModel = viewModel;
            ProductBeingModified = Inventory.LookupProduct(_homeViewModel.SelectedProduct.ProductID);
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            AssignFormProperties();
            ValidateInput();
            EnableSave = !HasErrors;
            Debug.WriteLine($"Has errors: {HasErrors}");

            NavigateHomeCommand = new NavigateHomeCommand(navigationStore);
            AddAssociatedPartCommand = new AddAssociatedPartCommand(this);
            DeleteAssociatedPartCommand = new DeleteAssociatedPartCommand(this);
            SaveModifiedProductCommand = new SaveModifiedProductCommand(this);
            SearchPartCommand = new SearchPartCommand(this);
            CancelModifiedProductCommand = new CancelModifiedProductCommand(this);
        }

        private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            EnableSave = !HasErrors;
            ErrorsChanged?.Invoke(this, e);
        }

        private void AssignFormProperties()
        {
            ProductID = ProductBeingModified.ProductID;
            ProductName = ProductBeingModified.Name;
            ProductInventory = ProductBeingModified.Inventory.ToString();
            ProductPrice = ProductBeingModified.Price.ToString();
            ProductMin = ProductBeingModified.Min.ToString();
            ProductMax = ProductBeingModified.Max.ToString();

            foreach (Part part in ProductBeingModified.AssociatedParts.ToList())
            {
                AssociatedParts.Add(part);
            }
        }

        public IEnumerable GetErrors(string propertyName) => _errorsViewModel.GetErrors(propertyName);

        public void ValidateInput()
        {
            // When Min, Max, or Inventory are set, I need to clear all their errors and reassess
            _errorsViewModel.ClearErrors(nameof(ProductName));
            _errorsViewModel.ClearErrors(nameof(ProductPrice));
            _errorsViewModel.ClearErrors(nameof(ProductInventory));
            _errorsViewModel.ClearErrors(nameof(ProductMin));
            _errorsViewModel.ClearErrors(nameof(ProductMax));

            // Validate nulls
            if (string.IsNullOrWhiteSpace(_productName))
            {
                _errorsViewModel.AddError(nameof(ProductName), "A product name is required");
            }
            if (_productInventory < 0 || _productInventory == null)
            {
                _errorsViewModel.AddError(nameof(ProductInventory), "Value must be at least 0");
            }
            if (_productPrice < 0 || _productPrice == null)
            {
                _errorsViewModel.AddError(nameof(ProductPrice), "Value must be at least 0");
            }
            if (_productMin < 0 || _productMin == null)
            {
                _errorsViewModel.AddError(nameof(ProductMin), "Value must be at least 0");
            }
            if (_productMax < 0 || _productMax == null)
            {
                _errorsViewModel.AddError(nameof(ProductMax), "Value must be at least 0");
            }

            // Validate value ranges
            if (_productInventory > _productMax || _productInventory < _productMin)
            {
                _errorsViewModel.AddError(nameof(ProductInventory), "Inventory value must be greater than Min and less than Max");
            }
            if (_productInventory < 0)
            {
                _errorsViewModel.AddError(nameof(ProductInventory), "Inventory value must be at least 0");
            }
            if (_productMax < _productMin)
            {
                _errorsViewModel.AddError(nameof(ProductMax), "Max value must be more than Min");
            }
            if (_productMax < _productInventory)
            {
                _errorsViewModel.AddError(nameof(ProductMax), "Max value must be more than Inventory");
            }
            if (_productMin > _productMax)
            {
                _errorsViewModel.AddError(nameof(ProductMin), "Min value must be less than Max");
            }
            if (_productMin > _productInventory)
            {
                _errorsViewModel.AddError(nameof(ProductMin), "Min value must be less than Inventory");
            }
        }
    }
}
