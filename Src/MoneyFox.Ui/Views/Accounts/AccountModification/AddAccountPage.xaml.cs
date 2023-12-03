namespace MoneyFox.Ui.Views.Accounts.AccountModification;

using Common.Navigation;

public partial class AddAccountPage: IBindablePage
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
