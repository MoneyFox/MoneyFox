namespace MoneyFox.Ui.Views.Accounts;

public partial class DesktopAccountListPage
{
    public DesktopAccountListPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<AccountListViewModel>();
    }

    private AccountListViewModel ViewModel => (AccountListViewModel)BindingContext;

    protected override async void OnAppearing()
    {
        await ViewModel.OnAppearingAsync();
    }
}
