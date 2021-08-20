using InventoryManager.Stores;
using InventoryManager.ViewModels;
using System.Windows;

namespace InventoryManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NavigationStore navigationStore = new NavigationStore();
            navigationStore.CurrentViewModel = new HomeViewModel(navigationStore);
            DataContext = new MainWindowViewModel(navigationStore);
        }
    }
}
