namespace MoneyFox.Ui.Views.Payments.PaymentModification;

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
        base.OnAppearing();
        ViewModel.IsActive = true;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        ViewModel.IsActive = false;
    }
}
