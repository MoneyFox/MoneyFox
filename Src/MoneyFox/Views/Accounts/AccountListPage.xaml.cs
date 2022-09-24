namespace MoneyFox.Views.Accounts;

using ViewModels.Accounts;

public partial class AccountListPage
{
    public AccountListPage()
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
