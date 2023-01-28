// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MoneyFox.Ui.WinUI;

using Core.Common.Interfaces;
using Core.Interfaces;
using Microsoft.Identity.Client;
using Platforms.Windows.Src;

/// <summary>
///     Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : MauiWinUIApplication
{
    private const string MSAL_APPLICATION_ID = "00a3e4cd-b4b0-4730-be62-5fcf90a94a1d";

    /// <summary>
    ///     Initializes the singleton application object.  This is the first line of authored code
    ///     executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        InitializeComponent();
        Ui.App.AddPlatformServicesAction = AddServices;
    }

    protected override MauiApp CreateMauiApp()
    {
        return MauiProgram.CreateMauiApp();
    }

    private static void AddServices(IServiceCollection services)
    {
        _ = services.AddTransient<IAppInformation, WindowsAppInformation>();
        _ = services.AddTransient<IStoreOperations, MarketplaceOperations>();
        _ = services.AddTransient<IFileStore, WindowsFileStore>();
        _ = services.AddTransient<IDbPathProvider, DbPathProvider>();
        var publicClientApplication = PublicClientApplicationBuilder.Create(MSAL_APPLICATION_ID).WithRedirectUri($"msal{MSAL_APPLICATION_ID}://auth").Build();
        TokenCacheHelper.EnableSerialization(publicClientApplication.UserTokenCache);
        _ = services.AddSingleton(publicClientApplication);
    }
}
