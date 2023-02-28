namespace MoneyFox.Ui.Views.Accounts.AccountList;

public partial class DesktopAccountListPage
{
    public DesktopAccountListPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<AccountListViewModel>();
    }

    public AccountListViewModel ViewModel => (AccountListViewModel)BindingContext;

    protected override async void OnAppearing()
    {
        await ViewModel.InitializeAsync();
    }
}