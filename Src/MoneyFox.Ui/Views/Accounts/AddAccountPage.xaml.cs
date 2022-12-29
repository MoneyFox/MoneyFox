namespace MoneyFox.Ui.Views.Accounts;

public partial class AddAccountPage
{
    public AddAccountPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<AddAccountViewModel>();
    }

    private AddAccountViewModel ViewModel => (AddAccountViewModel)BindingContext;
}
