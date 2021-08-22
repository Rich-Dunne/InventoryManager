using GalaSoft.MvvmLight.CommandWpf;
using InventoryManager.Commands;
using InventoryManager.Models;
using InventoryManager.Stores;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace InventoryManager.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        #region Commands
        public ICommand NavigateAddProductCommand { get; }
        public ICommand NavigateAddPartCommand { get; }
        public ICommand DeleteProductCommand { get; }
        public RelayCommand<Window> CloseWindowCommand { get; private set; }
        #endregion

        private BaseViewModel _selectedViewModel;
        public BaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }

        public BindingList<Product> Products { get => Inventory.Products; }
        public BindingList<Part> Parts { get => Inventory.Parts; }

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

        // TODO:  Make Product search bar functional

        public HomeViewModel(NavigationStore navigationStore)
        {
            NavigateAddProductCommand = new NavigateAddProductCommand(navigationStore);
            NavigateAddPartCommand = new NavigateAddPartCommand(navigationStore);
            DeleteProductCommand = new DeleteProductCommand(this);
            CloseWindowCommand = new RelayCommand<Window>(CloseWindow);
        }

        private void CloseWindow(Window window) => window?.Close();
    }
}
