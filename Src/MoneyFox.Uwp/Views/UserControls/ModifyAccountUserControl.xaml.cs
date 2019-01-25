using Windows.UI.Xaml;
using MoneyFox.ServiceLayer.Utilities;

namespace MoneyFox.Uwp.Views.UserControls
{
    public sealed partial class ModifyAccountUserControl
    {
        public ModifyAccountUserControl()
        {
            InitializeComponent();
        }

        private void TextBoxOnFocus(object sender, RoutedEventArgs e)
        {
            TextBoxCurrentBalance.SelectAll();
        }

        private void FormatTextBoxOnLostFocus(object sender, RoutedEventArgs e)
        {
            double.TryParse(TextBoxCurrentBalance.Text, out var amount);
            TextBoxCurrentBalance.Text = Utilities.FormatLargeNumbers(amount);
        }
    }
}
