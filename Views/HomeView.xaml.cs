using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace InventoryManager.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void dg_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Selector selector = sender as Selector;
            DataGrid dataGrid = selector as DataGrid;
            if (dataGrid != null && selector.SelectedItem != null && dataGrid.SelectedIndex >= 0)
            {
                dataGrid.ScrollIntoView(selector.SelectedItem);
            }
        }
    }
}
