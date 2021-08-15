using System.Windows;
using System.Windows.Controls;

namespace InventoryManager.Views
{
    /// <summary>
    /// Interaction logic for ProductView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void btnAddProduct_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Add Product window should open here.");
        }
    }
}
