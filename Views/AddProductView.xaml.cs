using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace InventoryManager.Views
{
    /// <summary>
    /// Interaction logic for AddProductView.xaml
    /// </summary>
    public partial class AddProductView : UserControl
    {
        public AddProductView()
        {
            InitializeComponent();
        }

        private void PreviewIntInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsIntInputValid(e.Text);
        }

        private bool IsIntInputValid(string text)
        {
            var match = Regex.Match(text, @"^[\d]+$");
            return match.Success;
        }

        private void PreviewPriceInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsPriceInputValid(e.Text);
        }

        private bool IsPriceInputValid(string text)
        {
            var match = Regex.Match(text, @"^[1-9]\d{0,2}(\.\d{3})*(,\d+)?$");
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
    }
}
