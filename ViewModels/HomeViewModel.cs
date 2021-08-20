using GalaSoft.MvvmLight.CommandWpf;
using InventoryManager.Commands;
using InventoryManager.Models;
using InventoryManager.Stores;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace InventoryManager.ViewModels
{
    class HomeViewModel : BaseViewModel
    {
        public ICommand NavigateAddProductCommand { get; }
        public ICommand NavigateAddPartCommand { get; }

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

        public BindingList<Product> Products { get => Inventory.Products; set => OnPropertyChanged("Products"); }
        public BindingList<Part> Parts { get => Inventory.Parts; set => OnPropertyChanged("Parts"); }
        public RelayCommand<Window> CloseWindowCommand { get; private set; }

        public HomeViewModel(NavigationStore navigationStore)
        {
            NavigateAddProductCommand = new NavigateAddProductCommand(navigationStore);
            NavigateAddPartCommand = new NavigateAddPartCommand(navigationStore);
            CloseWindowCommand = new RelayCommand<Window>(CloseWindow);
        }

        private void CloseWindow(Window window) => window?.Close();
    }
}
