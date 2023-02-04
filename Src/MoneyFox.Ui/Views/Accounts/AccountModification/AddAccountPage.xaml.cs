namespace MoneyFox.Ui.Views.Accounts.AccountModification;

public partial class AddAccountPage
{
    public AddAccountPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<AddAccountViewModel>();
    }
}
