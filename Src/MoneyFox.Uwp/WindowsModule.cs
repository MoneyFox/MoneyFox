using Autofac;
using MoneyFox.Application.Common;
using MoneyFox.Infrastructure;
using MoneyFox.Presentation;
using MoneyFox.Uwp.Src;
using PCLAppConfig;

namespace MoneyFox.Uwp
{
    public class WindowsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new TokenObject { CurrencyConverterApi = ConfigurationManager.AppSettings["CurrencyConverterApiKey"] });

            builder.RegisterModule<PresentationModule>();
            builder.RegisterModule<InfrastructureModule>();

            builder.RegisterType<DialogService>().AsImplementedInterfaces();
            builder.RegisterType<WindowsAppInformation>().AsImplementedInterfaces();
            builder.RegisterType<MarketplaceOperations>().AsImplementedInterfaces();
            builder.RegisterType<WindowsFileStore>().AsImplementedInterfaces();
            builder.RegisterType<ThemeSelectorAdapter>().AsImplementedInterfaces();
        }
    }
}
