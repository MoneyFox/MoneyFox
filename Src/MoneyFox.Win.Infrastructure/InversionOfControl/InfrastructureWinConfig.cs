namespace MoneyFox.Win.Infrastructure.InversionOfControl;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using MoneyFox.Core.Interfaces;
using MoneyFox.Infrastructure.InversionOfControl;
using MoneyFox.Win.Infrastructure;
using MoneyFox.Win.Infrastructure.Adapters;

public sealed class InfrastructureWinConfig
{
    private const string MSAL_APPLICATION_ID = "00a3e4cd-b4b0-4730-be62-5fcf90a94a1d";

    public void Register(ServiceCollection serviceCollection)
    {
        RegisterAdapters(serviceCollection);
        RegisterIdentityClient(serviceCollection);
        InfrastructureConfig.Register(serviceCollection);
    }

    private static void RegisterAdapters(ServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IBrowserAdapter, BrowserAdapter>();
        serviceCollection.AddTransient<IConnectivityAdapter, ConnectivityAdapter>();
        serviceCollection.AddTransient<IEmailAdapter, EmailAdapter>();
        serviceCollection.AddTransient<ISettingsAdapter, SettingsAdapter>();
    }

    private static void RegisterIdentityClient(ServiceCollection serviceCollection)
    {
        var publicClientApplication = PublicClientApplicationBuilder.Create(MSAL_APPLICATION_ID).WithRedirectUri($"msal{MSAL_APPLICATION_ID}://auth").Build();
        TokenCacheHelper.EnableSerialization(publicClientApplication.UserTokenCache);
        serviceCollection.AddSingleton(publicClientApplication);
    }
}
