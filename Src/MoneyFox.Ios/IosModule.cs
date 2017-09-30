using Autofac;
using MoneyFox.Business;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Ios.OneDriveAuth;
using MoneyFox.Ios.Services;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;

namespace MoneyFox.Ios
{
    public class IosModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<BusinessModule>();

            builder.RegisterType<DialogService>().As<IDialogService>();
			//builder.RegisterType<ModifyDialogService>().As<IModifyDialogService>();
			builder.RegisterType<OneDriveAuthenticator>().As<IOneDriveAuthenticator>();
			builder.RegisterType<ProtectedData>().As<IProtectedData>();
			//builder.RegisterType<NotificationService>().As<INotificationService>();
			builder.RegisterType<BackgroundTaskManager>().As<IBackgroundTaskManager>();
            //builder.RegisterType<TileManager>().As<ITileManager>();

            //builder.RegisterType<DroidAppInformation>().As<IAppInformation>();
            //builder.RegisterType<PlayStoreOperations>().As<IStoreOperations>();
            builder.RegisterType<ConnectivityImplementation>().As<IConnectivity>();
        }
    }
}