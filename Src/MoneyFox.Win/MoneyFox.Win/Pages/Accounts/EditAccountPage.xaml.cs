namespace MoneyFox.Win.Pages.Accounts;

using Microsoft.UI.Xaml.Controls;
using ViewModels.Accounts;

public sealed partial class EditAccountPage : ContentDialog
{
    private EditAccountViewModel ViewModel => (EditAccountViewModel)DataContext;

    public EditAccountPage(int accountId)
    {
        XamlRoot = MainWindow.RootFrame.XamlRoot;
        InitializeComponent();
        DataContext = ViewModelLocator.EditAccountVm;
        ViewModel.AccountId = accountId;
    }
}