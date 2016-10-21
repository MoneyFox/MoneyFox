using Autofac;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Ios.Services;
using MvvmCross.iOS.Support;
using MvvmCross.iOS.Support.SidePanels;

namespace MoneyFox.Ios
{
    public class IosModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DialogService>().As<IDialogService>();
            //builder.RegisterType<OneDriveAuthenticator>().As<IOneDriveAuthenticator>();
            builder.RegisterType<ProtectedData>().As<IProtectedData>();
            builder.RegisterType<NotificationService>().As<INotificationService>();
            //builder.RegisterType<BackgroundTaskManager>().As<IBackgroundTaskManager>();

            builder.RegisterInstance(new MvxPanelPopToRootPresentationHint(MvxPanelEnum.Center));
        }
    }
}