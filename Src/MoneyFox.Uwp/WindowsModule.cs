using Autofac;
using GalaSoft.MvvmLight.Messaging;
using MediatR;
using Microsoft.Identity.Client;
using MoneyFox.Application;
using MoneyFox.Application.Common;
using MoneyFox.Application.Common.Adapters;
using MoneyFox.Application.Common.Constants;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.FileStore;
using MoneyFox.Application.Payments.Queries.GetPaymentById;
using MoneyFox.Persistence;
using MoneyFox.Uwp.AutoMapper;
using MoneyFox.Uwp.Src;
using PCLAppConfig;
using System;
using System.Globalization;

namespace MoneyFox.Uwp
{
    public class WindowsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new TokenObject { CurrencyConverterApi = ConfigurationManager.AppSettings["CurrencyConverterApiKey"] });

            builder.RegisterModule<ApplicationModule>();
            builder.RegisterModule<PersistenceModule>();

            builder.RegisterType<GraphClientFactory>().AsImplementedInterfaces();
            builder.RegisterType<LongRunningTaskRequester>().AsImplementedInterfaces();
            builder.RegisterType<DialogService>().AsImplementedInterfaces();
            builder.RegisterType<WindowsAppInformation>().AsImplementedInterfaces();
            builder.RegisterType<MarketplaceOperations>().AsImplementedInterfaces();
            builder.RegisterType<WindowsFileStore>().As<IFileStore>();
            builder.RegisterType<ThemeSelectorAdapter>().AsImplementedInterfaces();
            builder.Register((c) => Messenger.Default).AsImplementedInterfaces();

            builder.RegisterType<Mediator>().As<IMediator>().InstancePerLifetimeScope();

            builder.RegisterInstance(AutoMapperFactory.Create());

            builder.Register(c => PublicClientApplicationBuilder
                                 .Create(ServiceConstants.MSAL_APPLICATION_ID)
                                 .WithRedirectUri($"msal{ServiceConstants.MSAL_APPLICATION_ID}://auth")
                                 .Build())
                   .AsImplementedInterfaces();

            // request & notification handlers
            builder.Register<ServiceFactory>(context =>
                                             {
                                                 var c = context.Resolve<IComponentContext>();

                                                 return t => c.Resolve(t);
                                             });

            builder.RegisterAssemblyTypes(typeof(GetPaymentByIdQuery).Assembly).AsImplementedInterfaces(); // via assembly scan

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => t.Name.EndsWith("Service", StringComparison.CurrentCultureIgnoreCase))
                   .Where(t => !t.Name.Equals("NavigationService", StringComparison.CurrentCultureIgnoreCase))
                   .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => !t.Name.StartsWith("DesignTime", StringComparison.CurrentCultureIgnoreCase))
                   .Where(t => t.Name.EndsWith("ViewModel", StringComparison.CurrentCultureIgnoreCase))
                   .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => !t.Name.StartsWith("DesignTime", StringComparison.CurrentCultureIgnoreCase))
                   .Where(t => t.Name.EndsWith("ViewModel", StringComparison.CurrentCultureIgnoreCase))
                   .AsSelf();

            CultureHelper.CurrentCulture = CultureInfo.CreateSpecificCulture(new SettingsFacade(new SettingsAdapter()).DefaultCulture);
        }
    }
}
