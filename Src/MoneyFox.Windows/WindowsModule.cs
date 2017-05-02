using Autofac;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Windows.Business;
using MoneyFox.Windows.Services;

namespace MoneyFox.Windows
{
    public class WindowsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DialogService>().As<IDialogService>();
            builder.RegisterType<OneDriveAuthenticator>().As<IOneDriveAuthenticator>();
            builder.RegisterType<ProtectedData>().As<IProtectedData>();
            builder.RegisterType<NotificationService>().As<INotificationService>();
            builder.RegisterType<BackgroundTaskManager>().As<IBackgroundTaskManager>();
            builder.RegisterType<TileUpdateService>().As<ITileUpdateService>();
            builder.RegisterType<TileManager>().As<ITileManager>();
            builder.RegisterType<WindowsAppInformation>().As<IAppInformation>();
            builder.RegisterType<MarketplaceOperations>().As<IStoreOperations>();
        }
    }
}
