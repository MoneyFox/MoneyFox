// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MoneyFox.Ui.WinUI;

using Core.Interfaces;
using Infrastructure.DbBackup;
using Microsoft.UI.Xaml;
using MoneyFox.Core.Common.Interfaces;
using Win;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : MauiWinUIApplication
{
    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        InitializeComponent();
        Ui.App.AddPlatformServicesAction = AddServices;
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    private static void AddServices(IServiceCollection services)
    {
        services.AddTransient<IGraphClientFactory, GraphClientFactory>();
        services.AddTransient<IAppInformation, WindowsAppInformation>();
        services.AddTransient<IStoreOperations, MarketplaceOperations>();
        services.AddTransient<IFileStore, WindowsFileStore>();
        services.AddTransient<IDbPathProvider, DbPathProvider>();
    }
}

