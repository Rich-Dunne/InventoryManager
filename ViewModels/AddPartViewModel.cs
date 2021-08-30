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
    public class AddPartViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        private ErrorsViewModel _errorsViewModel;

        #region Commands
        public ICommand NavigateHomeCommand { get; }
        public ICommand SaveNewPartCommand { get; }
        #endregion

        #region Properties
        public BindingList<Part> Parts { get; set; } = Inventory.Parts;
        #endregion

        #region FormProperties
        public int PartID { get; } = -1;
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

        private bool _isOutsourcedPart = false;
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
        #endregion

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


        public AddPartViewModel(NavigationStore navigationStore)
        {
            NavigateHomeCommand = new NavigateCommand<HomeViewModel>(navigationStore, () => new HomeViewModel(navigationStore));
            SaveNewPartCommand = new SaveNewPartCommand(this);
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            ValidateInput();
            PartID = GetNewPartID();
        }

        private int GetNewPartID()
        {
            if (Inventory.DeletedPartIDs.Count > 0)
            {
                return Inventory.DeletedPartIDs.First();
            }
            else if (Inventory.Parts.Count > 0)
            {
                return Inventory.Parts.Last().PartID + 1;
            }
            else
            {
                return 0;
            }
        }

        public IEnumerable GetErrors(string propertyName) => _errorsViewModel.GetErrors(propertyName);

        private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            EnableSave = !HasErrors;
            ErrorsChanged?.Invoke(this, e);
        }

        private void ValidateInput()
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
                _errorsViewModel.AddError(nameof(PartName), "A part name is required");
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
