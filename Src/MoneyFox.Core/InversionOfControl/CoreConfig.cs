namespace MoneyFox.Core.InversionOfControl;

using Common.Facades;
using Common.Helpers;
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
        _ = serviceCollection.AddTransient<ISystemDateHelper, SystemDateHelper>();
    }

    private static void RegisterMediatr(IServiceCollection serviceCollection)
    {
        _ = serviceCollection.AddMediatR(typeof(CustomMediator));
        _ = serviceCollection.AddTransient<ICustomPublisher, CustomPublisher>();
    }

    private static void RegisterFacades(IServiceCollection serviceCollection)
    {
        _ = serviceCollection.AddTransient<ISettingsFacade, SettingsFacade>();
        _ = serviceCollection.AddTransient<IConnectivityFacade, ConnectivityFacade>();
    }
}
