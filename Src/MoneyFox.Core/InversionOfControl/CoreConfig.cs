namespace MoneyFox.Core.InversionOfControl
{

    using _Pending_.Common.Facades;
    using Common;
    using Common.Mediatr;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;

    public sealed class CoreConfig
    {
        public void Register(ServiceCollection serviceCollection)
        {
            RegisterHelper(serviceCollection);
            RegisterMediatr(serviceCollection);
            RegisterFacades(serviceCollection);
        }

        private static void RegisterHelper(ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ISystemDateHelper, SystemDateHelper>();
        }

        private static void RegisterMediatr(ServiceCollection serviceCollection)
        {
            serviceCollection.AddMediatR(typeof(CustomMediator));
            serviceCollection.AddTransient<ICustomPublisher, CustomPublisher>();
        }

        private static void RegisterFacades(ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ISettingsFacade, SettingsFacade>();
            serviceCollection.AddTransient<IConnectivityFacade, ConnectivityFacade>();
        }
    }

}
