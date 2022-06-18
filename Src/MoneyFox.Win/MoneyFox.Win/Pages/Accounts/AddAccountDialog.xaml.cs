namespace MoneyFox.Win.Pages.Accounts;

using ViewModels.Accounts;

public sealed partial class AddAccountDialog
{
    public AddAccountDialog()
    {
        XamlRoot = MainWindow.RootFrame.XamlRoot;
        InitializeComponent();
        DataContext = App.GetViewModel<AddAccountViewModel>();
    }
}
