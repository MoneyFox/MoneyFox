namespace MoneyFox.Ui.Views.Accounts.AccountModification;

public partial class AddAccountPage
{
    public AddAccountPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<AddAccountViewModel>();
    }

    private AddAccountViewModel ViewModel => (AddAccountViewModel)BindingContext;

    protected override void OnAppearing()
    {
        ViewModel.IsActive = true;
    }

    protected override void OnDisappearing()
    {
        ViewModel.IsActive = false;
    }
}
