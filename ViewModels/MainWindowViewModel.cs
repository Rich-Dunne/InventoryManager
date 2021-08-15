using System.ComponentModel;
using InventoryManager.Models;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using System;

namespace InventoryManager.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public BindingList<Product> Products { get => Inventory.Products; set => OnPropertyChanged("Products"); }
        public BindingList<Part> Parts { get => Inventory.Parts; set => OnPropertyChanged("Parts"); }

        public MainWindowViewModel()
        {
            Inventory.InitializeData();
        }

        private void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
