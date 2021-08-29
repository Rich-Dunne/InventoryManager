﻿using GalaSoft.MvvmLight.CommandWpf;
using InventoryManager.Commands;
using InventoryManager.Models;
using InventoryManager.Stores;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace InventoryManager.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        #region Commands
        public ICommand NavigateAddProductCommand { get; }
        public ICommand NavigateModifyProductCommand { get; }
        public ICommand DeleteProductCommand { get; }
        public ICommand SearchProductCommand { get; }
        public ICommand NavigateAddPartCommand { get; }
        public ICommand NavigateModifyPartCommand { get; }
        public ICommand DeletePartCommand { get; }
        public ICommand SearchPartCommand { get; }
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

        public ObservableCollection<Product> Products 
        { 
            get => Inventory.Products; 
            set
            {
                Inventory.Products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        public ObservableCollection<Part> Parts 
        {
            get => Inventory.Parts; 
            set
            {
                Inventory.Parts = value;
                OnPropertyChanged(nameof(Parts));
            }
        }
        public string ProductSearchBoxContents { get; set; } = "";
        public string PartSearchBoxContents { get; set; } = "";

        private Part _selectedPart = null;
        public Part SelectedPart
        {
            get => _selectedPart;
            set
            {
                _selectedPart = value;
                OnPropertyChanged(nameof(SelectedPart));

                if(_selectedPart == null)
                {
                    PartSelected = false;
                }
                else
                {
                    PartSelected = true;
                    SelectedProduct = null;
                }
            }
        }

        private Product _selectedProduct = null;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));

                if (SelectedProduct == null)
                {
                    ProductSelected = false;
                }
                else
                {
                    ProductSelected = true;
                    SelectedPart = null;
                }
            }
        }

        private bool _productSelected = false;
        public bool ProductSelected
        {
            get => _productSelected;
            set
            {
                _productSelected = value;
                OnPropertyChanged(nameof(ProductSelected));
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

        public HomeViewModel(NavigationStore navigationStore)
        {
            SearchProductCommand = new SearchProductCommand(this);
            NavigateAddProductCommand = new NavigateAddProductCommand(navigationStore);
            NavigateModifyProductCommand = new NavigateModifyProductCommand(navigationStore, this);
            DeleteProductCommand = new DeleteProductCommand(this);

            SearchPartCommand = new SearchPartCommand(this);
            NavigateAddPartCommand = new NavigateAddPartCommand(navigationStore);
            NavigateModifyPartCommand = new NavigateModifyPartCommand(navigationStore, this);
            DeletePartCommand = new DeletePartCommand(this);
            CloseWindowCommand = new RelayCommand<Window>(CloseWindow);
        }

        private void CloseWindow(Window window) => window?.Close();
    }
}
