using Acr.UserDialogs;
using Autofac;
using GalaSoft.MvvmLight.Messaging;
using MoneyFox.Droid.Src;

#nullable enable
namespace MoneyFox.Droid
{
    public class AndroidModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GraphClientFactory>().AsImplementedInterfaces();
            builder.RegisterType<PlayStoreOperations>().AsImplementedInterfaces();
            builder.RegisterType<DroidAppInformation>().AsImplementedInterfaces();
            builder.RegisterType<UserDialogsImpl>().As<IUserDialogs>();
            builder.Register(c => new FileStoreIoBase(Android.App.Application.Context.FilesDir?.Path ?? "")).AsImplementedInterfaces();
            builder.RegisterInstance(Messenger.Default).AsImplementedInterfaces();

            builder.RegisterModule<MoneyFoxModule>();
        }
    }
}
