using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using MoneyFox.Business.Helpers;

namespace MoneyFox.Windows.Views
{
    /// <summary>
    ///     View to modify an account
    /// </summary>
    public sealed partial class ModifyAccountView
    {
        /// <summary>
        ///     Construtor
        /// </summary>
        public ModifyAccountView()
        {
            InitializeComponent();

            // code to handle bottom app bar when keyboard appears
            // workaround since otherwise the keyboard would overlay some controls
            InputPane.GetForCurrentView().Showing +=
                (s, args) => { BottomCommandBar.Visibility = Visibility.Collapsed; };
            InputPane.GetForCurrentView().Hiding += (s, args2) =>
            {
                if (BottomCommandBar.Visibility == Visibility.Collapsed)
                {
                    BottomCommandBar.Visibility = Visibility.Visible;
                }
            };
        }

        private void TextBoxOnFocus(object sender, RoutedEventArgs e)
        {
            TextBoxCurrentBalance.SelectAll();
        }

        private void FormatTextBoxOnLostFocus(object sender, RoutedEventArgs e)
        {
            double amount;
            double.TryParse(TextBoxCurrentBalance.Text, out amount);
            TextBoxCurrentBalance.Text = Utilities.FormatLargeNumbers(amount);
        }
    }
}