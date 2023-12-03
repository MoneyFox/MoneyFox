namespace MoneyFox.Ui.Views.Setup;

using Common.Navigation;

public partial class SetupAccountPage: IBindablePage
{
    public SetupAccountPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<SetupAccountsViewModel>();
    }
}
