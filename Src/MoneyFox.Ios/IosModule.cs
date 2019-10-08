using System;
using Autofac;
using MoneyFox.Infrastructure;
using MoneyFox.Presentation;

namespace MoneyFox.iOS
{
    public class IosModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<PresentationModule>();
            builder.RegisterModule<InfrastructureModule>();

            builder.RegisterType<DialogService>().AsImplementedInterfaces();
            builder.RegisterType<AppInformation>().AsImplementedInterfaces();
            builder.RegisterType<StoreOperations>().AsImplementedInterfaces();
            builder.RegisterType<BackgroundTaskManager>().AsImplementedInterfaces();
            builder.RegisterType<NavigationService>().AsImplementedInterfaces();
            builder.RegisterType<ThemeSelectorAdapter>().AsImplementedInterfaces();
            builder.Register(c => new IosFileStore(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))).AsImplementedInterfaces();
        }
    }
}
