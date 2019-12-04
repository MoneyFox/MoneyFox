using Autofac;
using MoneyFox.Application.FileStore;
using MoneyFox.Droid.Manager;
using MoneyFox.Infrastructure;
using MoneyFox.Presentation;

namespace MoneyFox.Droid
{
    public class AndroidModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<PresentationModule>();
            builder.RegisterModule<InfrastructureModule>();

            builder.RegisterType<DroidAppInformation>().AsImplementedInterfaces();
            builder.RegisterType<PlayStoreOperations>().AsImplementedInterfaces();
            builder.RegisterType<BackgroundTaskManager>().AsImplementedInterfaces();
            builder.RegisterType<NavigationService>().AsImplementedInterfaces();
            builder.RegisterType<ThemeSelectorAdapter>().AsImplementedInterfaces();
            builder.Register(c => new FileStoreIoBase(Android.App.Application.Context.FilesDir.Path)).AsImplementedInterfaces();
        }
    }
}
