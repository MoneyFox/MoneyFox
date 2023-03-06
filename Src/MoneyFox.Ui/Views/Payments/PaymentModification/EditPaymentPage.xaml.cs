namespace MoneyFox.Ui.Views.Payments.PaymentModification;

[QueryProperty(name: "PaymentId", queryId: "paymentId")]
public partial class EditPaymentPage
{
    public EditPaymentPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<EditPaymentViewModel>();
    }

    private EditPaymentViewModel ViewModel => (EditPaymentViewModel)BindingContext;

    protected override void OnAppearing()
    {
        ViewModel.IsActive = true;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        ViewModel.IsActive = false;
    }
}
