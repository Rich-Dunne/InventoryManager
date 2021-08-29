using System;
using System.Collections;
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
    public class ModifyPartViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        private HomeViewModel _homeViewModel;
        private ErrorsViewModel _errorsViewModel;

        #region Commands
        public ICommand NavigateHomeCommand { get; }
        public ICommand SaveNewPartCommand { get; }
        public ICommand SaveModifiedPartCommand { get; }
        #endregion

        #region Properties
        public BindingList<Part> Parts { get; set; } = Inventory.Parts;

        private Part _partBeingModified;
        public Part PartBeingModified
        {
            get => _partBeingModified;
            set
            {
                _partBeingModified = value;
                OnPropertyChanged(nameof(PartBeingModified));
            }
        }
        #endregion

        #region FormProperties
        public int PartID { get; set; } = -1;
        private string _partName;
        public string PartName
        {
            get => _partName;
            set
            {
                _partName = value;
                ValidateInput();
                OnPropertyChanged(nameof(PartName));
            }
        }

        private int _partInventory;
        public int PartInventory
        {
            get => _partInventory;

            set
            {
                _partInventory = value;
                ValidateInput();
                OnPropertyChanged(nameof(PartInventory));
            }
        }

        private double _partPrice;
        public double PartPrice
        {
            get => _partPrice;

            set
            {
                _partPrice = value;
                ValidateInput();
                OnPropertyChanged(nameof(PartPrice));
            }
        }

        private int _partMin;
        public int PartMin
        {
            get => _partMin;

            set
            {
                _partMin = value;
                ValidateInput();
                OnPropertyChanged(nameof(PartMin));
            }
        }

        private int _partMax;
        public int PartMax
        {
            get => _partMax;

            set
            {
                _partMax = value;
                ValidateInput();
                OnPropertyChanged(nameof(PartMax));
            }
        }

        private string _companyName;
        public string CompanyName
        {
            get => _companyName;
            set
            {
                _companyName = value;
                ValidateInput();
                OnPropertyChanged(nameof(CompanyName));
            }
        }

        private int _machineID;
        public int MachineID
        {
            get => _machineID;
            set
            {
                _machineID = value;
                ValidateInput();
                OnPropertyChanged(nameof(MachineID));
            }
        }

        private bool _isInHousePart = true;
        public bool IsInHousePart
        {
            get => _isInHousePart;
            set
            {
                _isInHousePart = value;
                ValidateInput();
                OnPropertyChanged(nameof(IsInHousePart));
            }
        }

        private bool _isOutsourcedPart = true;
        public bool IsOutsourcedPart
        {
            get => _isOutsourcedPart;
            set
            {
                _isOutsourcedPart = value;
                ValidateInput();
                OnPropertyChanged(nameof(IsOutsourcedPart));
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
        #endregion

        public ModifyPartViewModel(NavigationStore navigationStore, HomeViewModel viewModel)
        {
            NavigateHomeCommand = new NavigateHomeCommand(navigationStore);
            _homeViewModel = viewModel;
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;

            PartBeingModified = Inventory.LookupPart(_homeViewModel.SelectedPart.PartID);
            AssignFormProperties();
            EnableSave = !HasErrors;
            SaveModifiedPartCommand = new SaveModifiedPartCommand(this);
        }

        public IEnumerable GetErrors(string propertyName) => _errorsViewModel.GetErrors(propertyName);

        private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            EnableSave = !HasErrors;
            ErrorsChanged?.Invoke(this, e);
        }

        private void AssignFormProperties()
        {
            if(PartBeingModified == null)
            {
                Debug.WriteLine($"PartBeingModified is null.");
                return;
            }

            Debug.WriteLine($"Part being modified: {PartBeingModified.Name} (Type: {PartBeingModified.GetType().Name})");
            PartID = PartBeingModified.PartID;
            PartName = PartBeingModified.Name;
            PartInventory = PartBeingModified.Inventory;
            PartPrice = PartBeingModified.Price;
            PartMin = PartBeingModified.Min;
            PartMax = PartBeingModified.Max;
            if(PartBeingModified.GetType() == typeof(InHousePart))
            {
                IsInHousePart = true;
                IsOutsourcedPart = false;
                var inHousePart = PartBeingModified as InHousePart;
                MachineID = inHousePart.MachineID;
                CompanyName = "";
            }
            else
            {
                IsOutsourcedPart = true;
                IsInHousePart = false;
                var outsourcedPart = PartBeingModified as OutsourcedPart;
                CompanyName = outsourcedPart.CompanyName;
                MachineID = -1;
            }
        }

        public void ValidateInput()
        {
            // When Min, Max, or Inventory are set, I need to clear all their errors and reassess
            _errorsViewModel.ClearErrors(nameof(PartInventory));
            _errorsViewModel.ClearErrors(nameof(PartMin));
            _errorsViewModel.ClearErrors(nameof(PartMax));
            _errorsViewModel.ClearErrors(nameof(CompanyName));
            _errorsViewModel.ClearErrors(nameof(MachineID));
            _errorsViewModel.ClearErrors(nameof(PartPrice));
            _errorsViewModel.ClearErrors(nameof(PartName));
            if (string.IsNullOrWhiteSpace(_partName))
            {
                _errorsViewModel.AddError(nameof(PartName), "A product name is required");
            }
            if (_partPrice < 0)
            {
                _errorsViewModel.AddError(nameof(PartPrice), "Value must be at least 0");
            }
            if (_partInventory > PartMax || _partInventory < PartMin)
            {
                _errorsViewModel.AddError(nameof(PartInventory), "Inventory value must be greater than Min and less than Max");
            }
            if (_partInventory < 0)
            {
                _errorsViewModel.AddError(nameof(PartInventory), "Inventory value must be at least 0");
            }
            if (_partMax < PartMin)
            {
                _errorsViewModel.AddError(nameof(PartMax), "Max value must be more than Min");
            }
            if (_partMax < PartInventory)
            {
                _errorsViewModel.AddError(nameof(PartMax), "Max value must be more than Inventory");
            }
            if (_partMin > PartMax)
            {
                _errorsViewModel.AddError(nameof(PartMin), "Min value must be less than Max");
            }
            if (_partMin > PartInventory)
            {
                _errorsViewModel.AddError(nameof(PartMin), "Min value must be less than Inventory");
            }
            if (IsInHousePart && _machineID < 0)
            {
                _errorsViewModel.AddError(nameof(MachineID), "Value must be at least 0");
            }
            if (IsOutsourcedPart && string.IsNullOrWhiteSpace(_companyName))
            {
                _errorsViewModel.AddError(nameof(CompanyName), "A company name is required");
            }
        }
    }
}
