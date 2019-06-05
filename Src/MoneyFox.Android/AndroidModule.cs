using Autofac;
using MoneyFox.Droid.Manager;
using MoneyFox.Droid.Services;
using MoneyFox.ServiceLayer;
using MvvmCross.Plugin.File;

namespace MoneyFox.Droid
{
    public class AndroidModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<ServiceModule>();

            builder.RegisterType<DialogService>().AsImplementedInterfaces();
            builder.RegisterType<DroidAppInformation>().AsImplementedInterfaces();
            builder.RegisterType<PlayStoreOperations>().AsImplementedInterfaces();
            builder.RegisterType<BackgroundTaskManager>().AsImplementedInterfaces();
            builder.RegisterType<MvxIoFileStoreBase>().AsImplementedInterfaces();
        }
    }
}