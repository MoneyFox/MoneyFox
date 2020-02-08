using Autofac;
using MoneyFox.Application.Common;
using MoneyFox.Infrastructure;
using PCLAppConfig;

namespace MoneyFox.Uwp
{
    public class WindowsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new TokenObject {CurrencyConverterApi = ConfigurationManager.AppSettings["CurrencyConverterApiKey"]});

            builder.RegisterModule<InfrastructureModule>();

            builder.RegisterType<DialogService>().AsImplementedInterfaces();
            builder.RegisterType<WindowsAppInformation>().AsImplementedInterfaces();
            builder.RegisterType<MarketplaceOperations>().AsImplementedInterfaces();
            builder.RegisterType<WindowsFileStore>().AsImplementedInterfaces();
            builder.RegisterType<ThemeSelectorAdapter>().AsImplementedInterfaces();
        }
    }
}
