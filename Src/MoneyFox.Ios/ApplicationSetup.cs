using MoneyFox.BusinessLogic.Backup;
using MoneyFox.iOS.Authentication;
using MoneyFox.Presentation;
using MoneyFox.ServiceLayer.Interfaces;
using MvvmCross;
using MvvmCross.Forms.Platforms.Ios.Core;
using MvvmCross.IoC;
using MvvmCross.Logging;
using Serilog;
using Serilog.Events;

namespace MoneyFox.iOS
{
    public class ApplicationSetup : MvxFormsIosSetup<CoreApp, App>
    {
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IDialogService, DialogService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IOneDriveAuthenticator, OneDriveAuthenticator>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IProtectedData, ProtectedData>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IAppInformation, AppInformation>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IStoreOperations, StoreOperations>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IBackgroundTaskManager, BackgroundTaskManager>();
        }

        public override MvxLogProviderType GetDefaultLogProviderType() => MvxLogProviderType.Serilog;

        protected override IMvxLogProvider CreateLogProvider()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.NSLog()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Month)
                .CreateLogger();

            return base.CreateLogProvider();
        }
    }
}