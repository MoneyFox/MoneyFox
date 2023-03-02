namespace MoneyFox.Ui.Views.Payments.PaymentModification;

public partial class AddPaymentPage
{
    public AddPaymentPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<AddPaymentViewModel>();
    }
}
