#region

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MoneyManager.Views;

#endregion

namespace MoneyManager.UserControls
{
    public sealed partial class AddAccountUserControl
    {
        public AddAccountUserControl()
        {
            InitializeComponent();
        }

        private void RemoveZeroOnFocus(object sender, RoutedEventArgs e)
        {
            if (TextBoxCurrentBalance.Text == "0")
            {
                TextBoxCurrentBalance.Text = string.Empty;
            }

            TextBoxCurrentBalance.SelectAll();
        }

        private void AddZeroIfEmpty(object sender, RoutedEventArgs e)
        {
            if (TextBoxCurrentBalance.Text == string.Empty)
            {
                TextBoxCurrentBalance.Text = "0";
            }
        }

        private void OpenSelectCurrencyDialog(object sender, RoutedEventArgs e)
        {
            ((Frame) Window.Current.Content).Navigate(typeof (SelectCurrency));
        }
    }
}