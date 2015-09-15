using Windows.UI.Xaml.Controls;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsUWP.Platform;
using MoneyManager.Foundation.OperationContracts;
using MoneyManager.Foundation.OperationContracts.Shotcuts;
using MoneyManager.Windows.Services;
using MoneyManager.Windows.Shortcut;
using SQLite.Net.Interop;
using SQLite.Net.Platform.WinRT;
using Xamarin;

namespace MoneyManager.Windows
{
    public class Setup : MvxWindowsSetup
    {
        public Setup(Frame frame)
            : base(frame)
        {
        }

        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            Mvx.RegisterType<ISQLitePlatform, SQLitePlatformWinRT>();
            Mvx.RegisterType<IDatabasePath, DatabasePath>();
            Mvx.RegisterType<IDialogService, DialogService>();
            Mvx.RegisterType<IAppInformation, AppInformation>();
            Mvx.RegisterType<IStoreFeatures, StoreFeatures>();
            Mvx.RegisterType<IBackupService, OneDriveBackupService>();
            Mvx.RegisterType<IRoamingSettings, RoamingSettings>();

            Mvx.RegisterType<ISpendingShortcut, SpendingTile>();
            Mvx.RegisterType<IIncomeShortcut, IncomeTile>();
            Mvx.RegisterType<ITransferShortcut, TransferTile>();
        }

        protected override IMvxApplication CreateApp()
        {
            var insightKey = "e5c4ac56bb1ca47559bc8d4973d0a8c4d78c7648";

#if DEBUG
            insightKey = Insights.DebugModeKey;
#endif
            if (!Insights.IsInitialized)
            {
                Insights.Initialize(insightKey);
            }

            return new Core.App();
        }
    }
}