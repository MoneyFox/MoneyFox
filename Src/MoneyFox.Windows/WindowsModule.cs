using System;
using Autofac;
using MoneyFox.Presentation;
using MoneyFox.Uwp.Business;

namespace MoneyFox.Windows
{
    public class WindowsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<PresentationModule>();

            builder.RegisterType<DialogService>().AsImplementedInterfaces();
            builder.RegisterType<OneDriveAuthenticator>().AsImplementedInterfaces();
            builder.RegisterType<ProtectedData>().AsImplementedInterfaces();
            builder.RegisterType<WindowsAppInformation>().AsImplementedInterfaces();
            builder.RegisterType<MarketplaceOperations>().AsImplementedInterfaces();
            builder.RegisterType<BackgroundTaskManager>().AsImplementedInterfaces();
        }
    }
}
