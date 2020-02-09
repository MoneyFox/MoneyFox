using System;
using Autofac;
using MoneyFox.Application.Common;
using MoneyFox.Infrastructure;
using MoneyFox.Presentation;
using MoneyFox.Presentation.Services;
using PCLAppConfig;

namespace MoneyFox.iOS
{
    public class IosModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new TokenObject {CurrencyConverterApi = ConfigurationManager.AppSettings["CurrencyConverterApiKey"]});

            builder.RegisterModule<PresentationModule>();
            builder.RegisterModule<InfrastructureModule>();

            builder.RegisterType<AppInformation>().AsImplementedInterfaces();
            builder.RegisterType<StoreOperations>().AsImplementedInterfaces();
            builder.RegisterType<NavigationService>().AsImplementedInterfaces();
            builder.RegisterType<ThemeSelectorAdapter>().AsImplementedInterfaces();
            builder.Register(c => new IosFileStore(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)))
                   .AsImplementedInterfaces();
        }
    }
}
