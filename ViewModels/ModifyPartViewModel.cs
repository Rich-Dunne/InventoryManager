using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using InventoryManager.Commands;
using InventoryManager.Interfaces;
using InventoryManager.Models;
using InventoryManager.Stores;

namespace InventoryManager.ViewModels
{
    public class ModifyPartViewModel : BaseViewModel, INotifyDataErrorInfo, IPartViewModel
    {
        private ErrorsViewModel _errorsViewModel;

        #region Commands
        public ICommand NavigateHomeCommand { get; }
        public ICommand SavePartCommand { get; }
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

        private int? _partInventory;
        public string PartInventory
        {
            get => _partInventory?.ToString();

            set
            {
                if (int.TryParse(value, out var number))
                {
                    _partInventory = number;
                }
                else
                {
                    _partInventory = null;
                }

                ValidateInput();
                OnPropertyChanged(nameof(PartInventory));
            }
        }

        private double? _partPrice;
        public string PartPrice
        {
            get => _partPrice?.ToString();

            set
            {
                if (double.TryParse(value, out var number))
                {
                    _partPrice = number;
                }
                else
                {
                    _partPrice = null;
                }
                ValidateInput();
                OnPropertyChanged(nameof(PartPrice));
            }
        }

        private int? _partMin;
        public string PartMin
        {
            get => _partMin.ToString();

            set
            {
                if (int.TryParse(value, out var number))
                {
                    _partMin = number;
                }
                else
                {
                    _partMin = null;
                }

                ValidateInput();
                OnPropertyChanged(nameof(PartMin));
            }
        }

        private int? _partMax;
        public string PartMax
        {
            get => _partMax.ToString();

            set
            {
                if (int.TryParse(value, out var number))
                {
                    _partMax = number;
                }
                else
                {
                    _partMax = null;
                }

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

        private int? _machineID;
        public string MachineID
        {
            get => _machineID.ToString();
            set
            {
                if (int.TryParse(value, out var number))
                {
                    _machineID = number;
                }
                else
                {
                    _machineID = null;
                }

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

        public ModifyPartViewModel(NavigationStore navigationStore, Part partBeingModified)
        {
            NavigateHomeCommand = new NavigateCommand<HomeViewModel>(new Services.NavigationService<HomeViewModel>(navigationStore, () => new HomeViewModel(navigationStore)));
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;

            PartBeingModified = Inventory.LookupPart(partBeingModified.PartID);
            AssignFormProperties();
            EnableSave = !HasErrors;
            SavePartCommand = new SavePartCommand(this);
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
            PartInventory = PartBeingModified.Inventory.ToString();
            PartPrice = PartBeingModified.Price.ToString();
            PartMin = PartBeingModified.Min.ToString();
            PartMax = PartBeingModified.Max.ToString();
            if(PartBeingModified.GetType() == typeof(InHousePart))
            {
                IsInHousePart = true;
                IsOutsourcedPart = false;
                var inHousePart = PartBeingModified as InHousePart;
                MachineID = inHousePart.MachineID.ToString();

            }
            else
            {
                IsOutsourcedPart = true;
                IsInHousePart = false;
                var outsourcedPart = PartBeingModified as OutsourcedPart;
                CompanyName = outsourcedPart.CompanyName;

            }
        }

        public void ValidateInput()
        {
            // When Min, Max, or Inventory are set, I need to clear all their errors and reassess
            _errorsViewModel.ClearErrors(nameof(PartName));
            _errorsViewModel.ClearErrors(nameof(PartInventory));
            _errorsViewModel.ClearErrors(nameof(PartPrice));
            _errorsViewModel.ClearErrors(nameof(PartMin));
            _errorsViewModel.ClearErrors(nameof(PartMax));
            _errorsViewModel.ClearErrors(nameof(CompanyName));
            _errorsViewModel.ClearErrors(nameof(MachineID));

            // Validate nulls
            if (string.IsNullOrWhiteSpace(_partName))
            {
                _errorsViewModel.AddError(nameof(PartName), "A product name is required");
            }
            if (_partInventory < 0 || _partInventory == null)
            {
                _errorsViewModel.AddError(nameof(PartInventory), "Value must be at least 0");
            }
            if (_partPrice < 0 || _partPrice == null)
            {
                _errorsViewModel.AddError(nameof(PartPrice), "Value must be at least 0");
            }
            if (_partMin < 0 || _partMin == null)
            {
                _errorsViewModel.AddError(nameof(PartMin), "Value must be at least 0");
            }
            if (_partMax < 0 || _partMax == null)
            {
                _errorsViewModel.AddError(nameof(PartMax), "Value must be at least 0");
            }
            if (IsInHousePart && (_machineID < 0 || _machineID == null))
            {
                _errorsViewModel.AddError(nameof(MachineID), "Value must be at least 0");
            }
            if (IsOutsourcedPart && string.IsNullOrWhiteSpace(_companyName))
            {
                _errorsViewModel.AddError(nameof(CompanyName), "A company name is required");
            }

            // Validate value ranges
            if (_partInventory > _partMax || _partInventory < _partMin)
            {
                _errorsViewModel.AddError(nameof(PartInventory), "Inventory value must be greater than Min and less than Max");
            }
            if (_partMax < _partMin)
            {
                _errorsViewModel.AddError(nameof(PartMax), "Max value must be more than Min");
            }
            if (_partMax < _partInventory)
            {
                _errorsViewModel.AddError(nameof(PartMax), "Max value must be more than Inventory");
            }
            if (_partMin > _partMax)
            {
                _errorsViewModel.AddError(nameof(PartMin), "Min value must be less than Max");
            }
            if (_partMin > _partInventory)
            {
                _errorsViewModel.AddError(nameof(PartMin), "Min value must be less than Inventory");
            }
        }
    }
}
