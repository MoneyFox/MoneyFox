namespace MoneyFox.Mobile.Infrastructure.InversionOfControl
{

    using Adapters;
    using Core.Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Identity.Client;
    using MoneyFox.Infrastructure.InversionOfControl;

    public sealed class InfrastructureMobileConfig
    {
        private const string MsalApplicationId = "00a3e4cd-b4b0-4730-be62-5fcf90a94a1d";

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
            var publicClientApplication = PublicClientApplicationBuilder.Create(MsalApplicationId)
                .WithRedirectUri($"msal{MsalApplicationId}://auth")
                .WithIosKeychainSecurityGroup("com.microsoft.adalcache")
                .Build();

            serviceCollection.AddSingleton(publicClientApplication);
        }
    }

}
