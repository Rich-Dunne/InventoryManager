using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace InventoryManager.Views
{
    /// <summary>
    /// Interaction logic for ModifyProductView.xaml
    /// </summary>
    public partial class ModifyProductView : UserControl
    {
        public ModifyProductView()
        {
            InitializeComponent();
        }

        private void PreviewIntInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsIntInputValid(e.Text);
        }

        private bool IsIntInputValid(string text)
        {
            var match = Regex.Match(text, @"^[\S\d]+$");
            return match.Success;
        }

        private void TextBox_IntPasting(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }

        private void TextBox_PricePasting(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
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
