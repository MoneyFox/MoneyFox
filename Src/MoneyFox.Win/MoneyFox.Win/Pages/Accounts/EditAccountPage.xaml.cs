namespace MoneyFox.Win.Pages.Accounts;

using Microsoft.UI.Xaml.Controls;
using ViewModels.Accounts;

public sealed partial class EditAccountPage : ContentDialog
{
    public EditAccountPage(int accountId)
    {
        XamlRoot = MainWindow.RootFrame.XamlRoot;
        InitializeComponent();
        DataContext = App.GetViewModel<EditAccountViewModel>();
        ViewModel.AccountId = accountId;
    }

    private EditAccountViewModel ViewModel => (EditAccountViewModel)DataContext;
}
