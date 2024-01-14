namespace MoneyFox.Ui.Views.Accounts.AccountList;

using Common.Navigation;

public partial class DesktopAccountListPage : IBindablePage
{
    public DesktopAccountListPage()
    {
        InitializeComponent();
    }

    public AccountListViewModel ViewModel => (AccountListViewModel)BindingContext;
}
