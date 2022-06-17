namespace MoneyFox.Mobile.Infrastructure.InversionOfControl
{

    using Adapters;
    using Core.Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using MoneyFox.Infrastructure.InversionOfControl;

    public sealed class InfrastructureMobileConfig
    {
        public void Register(ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IBrowserAdapter, BrowserAdapter>();
            serviceCollection.AddTransient<IConnectivityAdapter, ConnectivityAdapter>();
            serviceCollection.AddTransient<IEmailAdapter, EmailAdapter>();
            serviceCollection.AddTransient<ISettingsAdapter, SettingsAdapter>();

            new InfrastructureConfig().Register(serviceCollection);
        }
    }

}
