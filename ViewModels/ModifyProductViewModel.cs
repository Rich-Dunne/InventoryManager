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

        //private ObservableCollection<Part> _associatedParts;
        public BindingList<Part> AssociatedParts { get; set; } = new BindingList<Part>();
        //{ 
        //    get => _associatedParts; 
        //    set
        //    {
        //        _associatedParts = value;
        //        OnPropertyChanged(nameof(AssociatedParts));
        //    }
        //}
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
            ProductInventory = ProductBeingModified.Inventory;
            ProductPrice = ProductBeingModified.Price;
            ProductMin = ProductBeingModified.Min;
            ProductMax = ProductBeingModified.Max;

            foreach (Part part in ProductBeingModified.AssociatedParts.ToList())
            {
                AssociatedParts.Add(part);
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
