namespace MoneyFox.Core.InversionOfControl;

using Common;
using Common.Settings;
using Features._Legacy_.Payments.CreatePayment;
using MediatR.NotificationPublishers;
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
                cfg.RegisterServicesFromAssembly(typeof(CreatePaymentCommand).Assembly);
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
