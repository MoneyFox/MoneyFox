using Autofac;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Ios.OneDriveAuth;
using MoneyFox.Ios.Services;
using MvvmCross.iOS.Support.SidePanels;

namespace MoneyFox.Ios
{
    public class IosModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
			builder.RegisterType<DialogService>().As<IDialogService>();
			//builder.RegisterType<ModifyDialogService>().As<IModifyDialogService>();
			builder.RegisterType<OneDriveAuthenticator>().As<IOneDriveAuthenticator>();
			builder.RegisterType<ProtectedData>().As<IProtectedData>();
			//builder.RegisterType<NotificationService>().As<INotificationService>();
			builder.RegisterType<BackgroundTaskManager>().As<IBackgroundTaskManager>();
			//builder.RegisterType<TileManager>().As<ITileManager>();

			//builder.RegisterType<DroidAppInformation>().As<IAppInformation>();
			//builder.RegisterType<PlayStoreOperations>().As<IStoreOperations>();

            builder.RegisterInstance(new MvxPanelPopToRootPresentationHint(MvxPanelEnum.Center));
        }
    }
}