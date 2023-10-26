namespace MoneyFox.Core.InversionOfControl;

using Common;
using Common.Settings;
using Features.PaymentCreation;
using Microsoft.Extensions.DependencyInjection;

public sealed class CoreConfig
{
    public void Register(IServiceCollection serviceCollection)
    {
        RegisterHelper(serviceCollection);
        RegisterFacades(serviceCollection);
        serviceCollection.AddMediatR(
            cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(CreatePayment.Command).Assembly);
                cfg.NotificationPublisher = new ParallelNoWaitPublisher();
                cfg.NotificationPublisherType = typeof(ParallelNoWaitPublisher);
            });
    }

    private static void RegisterHelper(IServiceCollection serviceCollection)
    {
        _ = serviceCollection.AddTransient<ISystemDateHelper, SystemDateHelper>();
    }

    private static void RegisterFacades(IServiceCollection serviceCollection)
    {
        _ = serviceCollection.AddTransient<ISettingsFacade, SettingsFacade>();
    }
}
