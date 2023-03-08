namespace MoneyFox.Ui.Views.Setup;

using MoneyFox.Ui.Views.Setup.SelectCurrency;

public partial class SetupShell
{
    public SetupShell()
    {
        InitializeComponent();
        RegisterSetupRoutes();
        GoToAsync(Routes.WelcomeViewRoute);
    }

    private static void RegisterSetupRoutes()
    {
        Routing.RegisterRoute(route: Routes.WelcomeViewRoute, type: typeof(WelcomePage));
        Routing.RegisterRoute(route: Routes.CurrencyIntroductionRoute, type: typeof(SetupCurrencyPage));
        Routing.RegisterRoute(route: Routes.SetupAccountsRoute, type: typeof(SetupAccountPage));
        Routing.RegisterRoute(route: Routes.CategoryIntroductionRoute, type: typeof(SetupCategoryPage));
        Routing.RegisterRoute(route: Routes.SetupCompletionRoute, type: typeof(SetupCompletionPage));
    }
}
