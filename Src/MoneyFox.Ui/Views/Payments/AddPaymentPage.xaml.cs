namespace MoneyFox.Ui.Views.Payments;

using ViewModels.Payments;

public partial class AddPaymentPage
{
    private int? defaultChargedAccountID;

    public AddPaymentPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<AddPaymentViewModel>();
    }

    private AddPaymentViewModel ViewModel => (AddPaymentViewModel)BindingContext;

    protected override async void OnAppearing()
    {
        await ViewModel.InitializeAsync(defaultChargedAccountID);
    }
}

