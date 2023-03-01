namespace MoneyFox.Ui.Views.Payments.PaymentModification;

[QueryProperty(name: "DefaultChargedAccountId", queryId: "defaultChargedAccountId")]
public partial class AddPaymentPage
{
    public AddPaymentPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<AddPaymentViewModel>();
    }

    private AddPaymentViewModel ViewModel => (AddPaymentViewModel)BindingContext;

    protected override void OnAppearing()
    {
        ViewModel.InitializeAsync(defaultChargedAccountId).GetAwaiter().GetResult();
    }

#pragma warning disable S2376 // Write-only properties should not be used
    private int? defaultChargedAccountId;
    public string DefaultChargedAccountId
    {
        set => defaultChargedAccountId = Convert.ToInt32(Uri.UnescapeDataString(value));
    }
#pragma warning restore S2376 // Write-only properties should not be used
}
