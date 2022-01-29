using Microsoft.UI.Xaml;

namespace MoneyFox.Win.Pages.Accounts
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
    }
}