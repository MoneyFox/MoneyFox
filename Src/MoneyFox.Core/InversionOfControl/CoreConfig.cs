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
        RegisterMediatr(serviceCollection);
        RegisterFacades(serviceCollection);
    }

    private static void RegisterHelper(IServiceCollection serviceCollection)
    {
        _ = serviceCollection.AddTransient<ISystemDateHelper, SystemDateHelper>();
    }

    private static void RegisterMediatr(IServiceCollection serviceCollection)
    {
        _ = serviceCollection.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreatePaymentCommand).Assembly);
            cfg.NotificationPublisher = new TaskWhenAllPublisher();
            cfg.NotificationPublisherType = typeof(TaskWhenAllPublisher);
        });
    }

    private static void RegisterFacades(IServiceCollection serviceCollection)
    {
        _ = serviceCollection.AddTransient<ISettingsFacade, SettingsFacade>();
    }
}
