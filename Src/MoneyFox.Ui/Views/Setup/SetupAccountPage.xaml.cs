namespace MoneyFox.Ui.Views.Setup;

public partial class SetupAccountPage
{
    public SetupAccountPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<SetupAccountsViewModel>();
    }
}
