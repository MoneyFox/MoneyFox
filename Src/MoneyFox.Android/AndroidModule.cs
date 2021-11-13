using Acr.UserDialogs;
using Autofac;
using CommunityToolkit.Mvvm.Messaging;

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
            builder.Register(c => UserDialogs.Instance).As<IUserDialogs>();
            builder.Register(c => new FileStoreIoBase(Android.App.Application.Context.FilesDir?.Path ?? ""))
                .AsImplementedInterfaces();

            builder.RegisterModule<MoneyFoxModule>();
        }
    }
}