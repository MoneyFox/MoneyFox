using Autofac;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Droid.Src.Widgets;
using MoneyManager.Foundation.Interfaces.Shotcuts;

namespace MoneyManager.Droid
{
    public class AndroidModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<DialogService>().As<IDialogService>().SingleInstance();
            builder.RegisterType<AppInformation>().As<IAppInformation>().SingleInstance();
            builder.RegisterType<StoreFeatures>().As<IStoreFeatures>().SingleInstance();
            builder.RegisterType<RoamingSettings>().As<IRoamingSettings>().SingleInstance();
            builder.RegisterType<LocalSettings>().As<ILocalSettings>().SingleInstance();
            builder.RegisterType<OneDriveAuthenticator>().As<IOneDriveAuthenticator>().SingleInstance();
            builder.RegisterType<ProtectedData>().As<IProtectedData>().SingleInstance();
            builder.RegisterType<ExpenseWidget>().As<ISpendingShortcut>().SingleInstance();
            builder.RegisterType<IncomeWidget>().As<IIncomeShortcut>().SingleInstance();
            builder.RegisterType<TransferWidget>().As<ITransferShortcut>().SingleInstance();
        }
    }
}