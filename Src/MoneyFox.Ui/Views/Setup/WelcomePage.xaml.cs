namespace MoneyFox.Ui.Views.Setup;

public partial class WelcomePage
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
