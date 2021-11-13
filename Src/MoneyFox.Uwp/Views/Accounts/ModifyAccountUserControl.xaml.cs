using Windows.UI.Xaml;

#nullable enable
namespace MoneyFox.Uwp.Views.Accounts
{
    public sealed partial class ModifyAccountUserControl
    {
        public ModifyAccountUserControl()
        {
            InitializeComponent();
        }

        private void TextBoxOnFocus(object sender, RoutedEventArgs e) => TextBoxCurrentBalance.SelectAll();
    }
}
