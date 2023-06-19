namespace MoneyFox.Ui.Views.Ledgers.LedgerModification;

public partial class AddLedgerPage
{
    public AddLedgerPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<AddLedgerViewModel>();
    }

    private AddLedgerViewModel ViewModel => (AddLedgerViewModel)BindingContext;

    protected override void OnAppearing()
    {
        ViewModel.IsActive = true;
    }

    protected override void OnDisappearing()
    {
        ViewModel.IsActive = false;
    }
}
