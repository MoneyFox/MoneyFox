using Autofac;
using MediatR;
using MoneyFox.Application;
using MoneyFox.Application.Common;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.FileStore;
using MoneyFox.Desktop.Infrastructure;
using MoneyFox.Desktop.Infrastructure.Adapters;
using MoneyFox.Infrastructure;
using MoneyFox.Uwp.AutoMapper;
using MoneyFox.Uwp.Services;
using PCLAppConfig;
using System;
using System.Globalization;

namespace MoneyFox.Uwp
{
    public class WindowsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(
                c =>
                    new TokenObject
                    {
                        CurrencyConverterApi = ConfigurationManager.AppSettings["CurrencyConverterApiKey"]
                    });

            builder.RegisterModule<ApplicationModule>();
            builder.RegisterModule<InfrastructureDesktop>();
            builder.RegisterModule<InfrastructureModule>();

            builder.RegisterType<GraphClientFactory>().AsImplementedInterfaces();
            builder.RegisterType<ToastService>().AsImplementedInterfaces();
            builder.RegisterType<DialogService>().AsImplementedInterfaces();
            builder.RegisterType<WindowsAppInformation>().AsImplementedInterfaces();
            builder.RegisterType<MarketplaceOperations>().AsImplementedInterfaces();
            builder.RegisterType<WindowsFileStore>().As<IFileStore>();
            builder.RegisterType<ThemeSelectorAdapter>().AsImplementedInterfaces();

            builder.RegisterType<Mediator>().As<IMediator>().InstancePerLifetimeScope();
            builder.RegisterInstance(AutoMapperFactory.Create());

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => t.Name.EndsWith("Service", StringComparison.CurrentCultureIgnoreCase))
                   .Where(t => !t.Name.Equals("NavigationService", StringComparison.CurrentCultureIgnoreCase))
                   .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => t.Name.Equals("NavigationService", StringComparison.CurrentCultureIgnoreCase))
                   .AsImplementedInterfaces()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => !t.Name.StartsWith("DesignTime", StringComparison.CurrentCultureIgnoreCase))
                   .Where(t => t.Name.EndsWith("ViewModel", StringComparison.CurrentCultureIgnoreCase))
                   .AsImplementedInterfaces()
                   .AsSelf()
                   .AsSelf();

            CultureHelper.CurrentCulture =
                CultureInfo.CreateSpecificCulture(new SettingsFacade(new SettingsAdapter()).DefaultCulture);
        }
    }
}