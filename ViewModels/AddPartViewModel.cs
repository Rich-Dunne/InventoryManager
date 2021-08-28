using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        public ObservableCollection<Part> Parts { get; set; } = Inventory.Parts;
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
                _errorsViewModel.ClearErrors(nameof(PartName));
                if (string.IsNullOrWhiteSpace(_partName))
                {
                    _errorsViewModel.AddError(nameof(PartName), "A product name is required");
                }
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
                _errorsViewModel.ClearErrors(nameof(PartPrice));
                if (_partPrice < 0)
                {
                    _errorsViewModel.AddError(nameof(PartPrice), "Value must be at least 0");
                }
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
                OnPropertyChanged(nameof(IsInHousePart));
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public bool HasErrors => _errorsViewModel.HasErrors;
        #endregion

        public AddPartViewModel(NavigationStore navigationStore)
        {
            NavigateHomeCommand = new NavigateHomeCommand(navigationStore);
            SaveNewPartCommand = new SaveNewPartCommand(this);
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
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

        private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e) => ErrorsChanged?.Invoke(this, e);

        public void ValidateInput()
        {
            // When Min, Max, or Inventory are set, I need to clear all their errors and reassess
            _errorsViewModel.ClearErrors(nameof(PartInventory));
            _errorsViewModel.ClearErrors(nameof(PartMin));
            _errorsViewModel.ClearErrors(nameof(PartMax));
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
        }
    }
}
