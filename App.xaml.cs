using InventoryManager.Stores;
using InventoryManager.ViewModels;
using System.Windows;

namespace InventoryManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Inventory.InitializeData();
            NavigationStore navigationStore = new NavigationStore();
            navigationStore.CurrentViewModel = new HomeViewModel(navigationStore);
            MainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel(navigationStore)
            };
            //MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
