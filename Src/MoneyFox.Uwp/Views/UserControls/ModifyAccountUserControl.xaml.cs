using Windows.UI.Xaml;

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
    }
}
