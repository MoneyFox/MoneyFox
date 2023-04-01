namespace MoneyFox.Ui.Views.Setup;

using Accounts.AccountModification;
using Categories.ModifyCategory;
using SelectCurrency;

public partial class SetupShell
{
    public SetupShell()
    {
        InitializeComponent();
        RegisterSetupRoutes();
    }

    private static void RegisterSetupRoutes()
    {
        Routing.RegisterRoute(route: Routes.CurrencyIntroductionRoute, type: typeof(SetupCurrencyPage));
        Routing.RegisterRoute(route: Routes.SetupAccountsRoute, type: typeof(SetupAccountPage));
        Routing.RegisterRoute(route: Routes.CategoryIntroductionRoute, type: typeof(SetupCategoryPage));
        Routing.RegisterRoute(route: Routes.SetupCompletionRoute, type: typeof(SetupCompletionPage));
        Routing.RegisterRoute(route: Routes.AddAccountRoute, type: typeof(AddAccountPage));
        Routing.RegisterRoute(route: Routes.AddCategoryRoute, type: typeof(AddCategoryPage));
    }
}
