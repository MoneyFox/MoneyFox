// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MoneyFox.Ui.Platforms.Windows;

using Infrastructure.Adapters;
using Microsoft.Identity.Client;
using MoneyFox.Core.Common.Interfaces;
using MoneyFox.Core.Interfaces;
using MoneyFox.Infrastructure.DbBackup;
using Src;

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
        services.AddTransient<IAppInformation, WindowsAppInformation>();
        services.AddTransient<IStoreOperations, MarketplaceOperations>();
        services.AddTransient<IFileStore, WindowsFileStore>();
        services.AddTransient<IDbPathProvider, DbPathProvider>();
        services.AddTransient<IBrowserAdapter, BrowserAdapter>();
        services.AddTransient<IConnectivityAdapter, ConnectivityAdapter>();
        services.AddTransient<IEmailAdapter, EmailAdapter>();
        services.AddTransient<ISettingsAdapter, SettingsAdapter>();

        var publicClientApplication = PublicClientApplicationBuilder.Create(MSAL_APPLICATION_ID).WithRedirectUri($"msal{MSAL_APPLICATION_ID}://auth").Build();
        TokenCacheHelper.EnableSerialization(publicClientApplication.UserTokenCache);
        services.AddSingleton(publicClientApplication);
    }
}
