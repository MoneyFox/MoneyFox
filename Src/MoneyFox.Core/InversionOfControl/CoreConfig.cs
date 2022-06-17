namespace MoneyFox.Core.InversionOfControl
{

    using _Pending_.Common.Facades;
    using Common.Mediatr;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;

    public sealed class CoreConfig
    {
        public void Register(ServiceCollection serviceCollection)
        {
            serviceCollection.AddMediatR(configuration: options => options.Using<CustomMediator>().AsSingleton(), typeof(CoreConfig));
            RegisterFacades(serviceCollection);
        }

        private static void RegisterFacades(ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ISettingsFacade, SettingsFacade>();
            serviceCollection.AddTransient<IConnectivityFacade, ConnectivityFacade>();
        }
    }

}
