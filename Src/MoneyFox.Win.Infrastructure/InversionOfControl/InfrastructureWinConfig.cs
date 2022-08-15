namespace MoneyFox.Win.Infrastructure.InversionOfControl;

using Adapters;
using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using MoneyFox.Infrastructure.InversionOfControl;

public sealed class InfrastructureWinConfig
{
    private const string MSAL_APPLICATION_ID = "00a3e4cd-b4b0-4730-be62-5fcf90a94a1d";

    public void Register(IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IDbPathProvider, DbPathProvider>();
        RegisterAdapters(serviceCollection);
        RegisterIdentityClient(serviceCollection);
        InfrastructureConfig.Register(serviceCollection);
    }

    private static void RegisterAdapters(IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IBrowserAdapter, BrowserAdapter>();
        serviceCollection.AddTransient<IConnectivityAdapter, ConnectivityAdapter>();
        serviceCollection.AddTransient<IEmailAdapter, EmailAdapter>();
        serviceCollection.AddTransient<ISettingsAdapter, SettingsAdapter>();
    }

    private static void RegisterIdentityClient(IServiceCollection serviceCollection)
    {
        var publicClientApplication = PublicClientApplicationBuilder.Create(MSAL_APPLICATION_ID).WithRedirectUri($"msal{MSAL_APPLICATION_ID}://auth").Build();
        TokenCacheHelper.EnableSerialization(publicClientApplication.UserTokenCache);
        serviceCollection.AddSingleton(publicClientApplication);
    }
}
