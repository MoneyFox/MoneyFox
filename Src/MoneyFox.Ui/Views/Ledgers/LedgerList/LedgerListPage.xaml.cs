namespace MoneyFox.Ui.Views.Ledgers.LedgerList;

public partial class LedgerListPage
{
    public LedgerListPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<LedgerListViewModel>();
    }

    public LedgerListViewModel ViewModel => (LedgerListViewModel)BindingContext;

    protected override void OnAppearing()
    {
        ViewModel.IsActive = true;
        ViewModel.InitializeAsync().GetAwaiter().GetResult();
    }

    protected override void OnDisappearing()
    {
        ViewModel.IsActive = false;
    }
}
