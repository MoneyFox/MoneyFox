using Autofac;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Interfaces.Shotcuts;
using MoneyManager.Windows.Concrete;
using MoneyManager.Windows.Services;
using MoneyManager.Windows.Shortcut;

namespace MoneyManager.Windows
{
    public class WindowsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<DialogService>().As<IDialogService>().SingleInstance();
            builder.RegisterType<AppInformation>().As<IAppInformation>().SingleInstance();
            builder.RegisterType<StoreFeatures>().As<IStoreFeatures>().SingleInstance();
            builder.RegisterType<RoamingSettings>().As<IRoamingSettings>().SingleInstance();
            builder.RegisterType<LocalSettings>().As<ILocalSettings>().SingleInstance();
            builder.RegisterType<UserNotification>().As<IUserNotification>().SingleInstance();
            builder.RegisterType<OneDriveAuthenticator>().As<IOneDriveAuthenticator>().SingleInstance();
            builder.RegisterType<ProtectedData>().As<IProtectedData>().SingleInstance();

            builder.RegisterType<SpendingTile>().As<ISpendingShortcut>().SingleInstance();
            builder.RegisterType<IncomeTile>().As<IIncomeShortcut>().SingleInstance();
            builder.RegisterType<TransferTile>().As<ITransferShortcut>().SingleInstance();
        }
    }
}