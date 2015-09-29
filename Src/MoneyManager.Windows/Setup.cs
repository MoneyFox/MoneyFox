using Windows.UI.Xaml.Controls;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Plugins.Email;
using Cirrious.MvvmCross.Plugins.Email.WindowsCommon;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsUWP.Platform;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Interfaces.Shotcuts;
using MoneyManager.Windows.Services;
using MoneyManager.Windows.Shortcut;
using MvvmCross.Plugins.Sqlite;
using MvvmCross.Plugins.Sqlite.WindowsUWP;
using MvvmCross.Plugins.WebBrowser;
using MvvmCross.Plugins.WebBrowser.WindowsCommon;
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
            Mvx.RegisterType<IDialogService, DialogService>();
            Mvx.RegisterType<IAppInformation, AppInformation>();
            Mvx.RegisterType<IStoreFeatures, StoreFeatures>();
            Mvx.RegisterType<IBackupService, OneDriveBackupService>();
            Mvx.RegisterType<IRoamingSettings, RoamingSettings>();
            Mvx.RegisterType<IUserNotification, UserNotification>();

            Mvx.RegisterType<ISpendingShortcut, SpendingTile>();
            Mvx.RegisterType<IIncomeShortcut, IncomeTile>();
            Mvx.RegisterType<ITransferShortcut, TransferTile>();
        }

        public override void LoadPlugins(IMvxPluginManager pluginManager)
        {
            base.LoadPlugins(pluginManager);

            Mvx.RegisterType<IMvxComposeEmailTask, MvxComposeEmailTask>();
            Mvx.RegisterType<IMvxWebBrowserTask, MvxWebBrowserTask>();
            Mvx.RegisterType<IMvxSqliteConnectionFactory, WindowsSqliteConnectionFactory>();
        }

        protected override IMvxApplication CreateApp()
        {
            var insightKey = "599ff6bfdc79368ff3d5f5629a57c995fe93352e";

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