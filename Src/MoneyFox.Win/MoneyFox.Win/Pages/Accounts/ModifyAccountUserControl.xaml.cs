namespace MoneyFox.Win.Pages.Accounts;

using Microsoft.UI.Xaml;

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
