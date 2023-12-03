namespace MoneyFox.Ui.Views.Payments.PaymentModification;

using Common.Navigation;

public partial class AddPaymentPage : IBindablePage
{
    public AddPaymentPage()
    {
        InitializeComponent();
    }

    private AddPaymentViewModel ViewModel => (AddPaymentViewModel)BindingContext;

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ViewModel.IsActive = true;
        ViewModel.CategorySelectionViewModel.IsActive = true;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        ViewModel.IsActive = false;
        ViewModel.CategorySelectionViewModel.IsActive = false;
    }
}
