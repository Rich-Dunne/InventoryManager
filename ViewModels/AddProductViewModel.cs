using System;
using System.Collections;
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
                if (_productPrice > 50)
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
                //_errorsViewModel.ClearErrors(nameof(ProductMin));
                //if (_productMin > ProductMax)
                //{
                //    _errorsViewModel.AddError(nameof(ProductMin), "Min must be less than Max");
                //}
                //if (_productMin > ProductInventory)
                //{
                //    _errorsViewModel.AddError(nameof(ProductMin), "Min must be less than Inventory");
                //}
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
                //_errorsViewModel.ClearErrors(nameof(ProductMax));
                //if (_productMax < ProductMin)
                //{
                //    _errorsViewModel.AddError(nameof(ProductMax), "Max must be more than Min");
                //}
                //if (_productMax < ProductInventory)
                //{
                //    _errorsViewModel.AddError(nameof(ProductMax), "Max must be more than Inventory");
                //}
                OnPropertyChanged(nameof(ProductMax));
            }
        }
        #endregion

        private object _selectedItem = null;
        public object SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }
        public string SearchBoxContents { get; set; } = "";

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public bool HasErrors => _errorsViewModel.HasErrors;

        // TODO:  Change selected item highlight color

        public AddProductViewModel(NavigationStore navigationStore)
        {
            NavigateHomeCommand = new NavigateHomeCommand(navigationStore);
            DeleteAssociatedPartCommand = new DeleteAssociatedPartCommand(this);
            AddAssociatedPartCommand = new AddAssociatedPartCommand(this);
            SaveNewProductCommand = new SaveNewProductCommand(this);
            SearchPartCommand = new SearchPartCommand(this);
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            ProductID = GetNewProductID();
        }

        private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
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
