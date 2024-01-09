namespace MoneyFox.Ui.Views.Setup;

using Common.Navigation;

public partial class WelcomePage : IBindablePage
{
    public WelcomePage(WelcomeViewModel welcomeViewModel)
    {
        InitializeComponent();
        BindingContext = welcomeViewModel;
    }
}
