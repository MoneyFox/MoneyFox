namespace MoneyFox.Ui.Views.Setup;

using Common.Navigation;

public partial class WelcomePage : IBindablePage
{
    public WelcomePage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<WelcomeViewModel>();
    }

    private WelcomeViewModel ViewModel => (WelcomeViewModel)BindingContext;

    protected override void OnAppearing()
    {
        ViewModel.InitAsync().GetAwaiter().GetResult();
    }
}
