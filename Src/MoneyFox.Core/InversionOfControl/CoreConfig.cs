namespace MoneyFox.Core.InversionOfControl
{

    using _Pending_.Common.Facades;
    using Common;
    using Common.Mediatr;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;

    public sealed class CoreConfig
    {
        public void Register(IServiceCollection serviceCollection)
        {
            RegisterHelper(serviceCollection);
            RegisterMediatr(serviceCollection);
            RegisterFacades(serviceCollection);
        }

        private static void RegisterHelper(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ISystemDateHelper, SystemDateHelper>();
        }

        private static void RegisterMediatr(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMediatR(typeof(CustomMediator));
            serviceCollection.AddTransient<ICustomPublisher, CustomPublisher>();
        }

        private static void RegisterFacades(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ISettingsFacade, SettingsFacade>();
            serviceCollection.AddTransient<IConnectivityFacade, ConnectivityFacade>();
        }
    }

}
