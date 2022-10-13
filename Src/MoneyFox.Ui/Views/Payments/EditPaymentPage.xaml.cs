namespace MoneyFox.Ui.Views.Payments;

using ViewModels.Payments;

[QueryProperty(name: "PaymentId", queryId: "paymentId")]
public partial class EditPaymentPage
{

    public EditPaymentPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<EditPaymentViewModel>();
    }

    private EditPaymentViewModel ViewModel => (EditPaymentViewModel)BindingContext;

#pragma warning disable S2376 // Write-only properties should not be used
    private int paymentId;
    public string PaymentId
    {
        set => paymentId = Convert.ToInt32(Uri.UnescapeDataString(value));
    }
#pragma warning restore S2376 // Write-only properties should not be used

    protected override async void OnAppearing()
    {
        await ViewModel.InitializeAsync(paymentId);
    }
}
