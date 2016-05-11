using Windows.UI.Xaml.Controls;
using Cheesebaron.MvxPlugins.Connectivity;
using Cheesebaron.MvxPlugins.Connectivity.WindowsCommon;
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using Cheesebaron.MvxPlugins.Settings.WindowsCommon;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Interfaces.Shotcuts;
using MoneyManager.Windows;
using MoneyManager.Windows.Services;
using MoneyManager.Windows.Shortcut;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.Plugins;
using MvvmCross.Platform.UI;
using MvvmCross.Plugins.Email;
using MvvmCross.Plugins.Email.WindowsCommon;
using MvvmCross.Plugins.File;
using MvvmCross.Plugins.File.WindowsCommon;
using MvvmCross.Plugins.Sqlite;
using MvvmCross.Plugins.Sqlite.WindowsUWP;
using MvvmCross.Plugins.Visibility.WindowsCommon;
using MvvmCross.Plugins.WebBrowser;
using MvvmCross.Plugins.WebBrowser.WindowsCommon;
using MvvmCross.WindowsUWP.Platform;
using PluginLoader = MvvmCross.Plugins.Messenger.PluginLoader;

namespace MoneyFox.Windows
{
    public class Setup : MvxWindowsSetup
    {
        public Setup(Frame frame)
            : base(frame)
        {
        }

        public override void LoadPlugins(IMvxPluginManager pluginManager)
        {
            base.LoadPlugins(pluginManager);
            pluginManager.EnsurePluginLoaded<PluginLoader>();

            //We have to do this here, since the loading via bootloader won't work for UWP projects
            Mvx.RegisterType<IMvxComposeEmailTask, MvxComposeEmailTask>();
            Mvx.RegisterType<IMvxWebBrowserTask, MvxWebBrowserTask>();
            Mvx.RegisterType<IMvxSqliteConnectionFactory, WindowsSqliteConnectionFactory>();
            Mvx.RegisterType<IMvxFileStore, MvxWindowsCommonFileStore>();
            Mvx.RegisterType<ISettings, WindowsCommonSettings>();
            Mvx.RegisterType<IConnectivity, Connectivity>();
            Mvx.RegisterType<IMvxNativeVisibility, MvxWinRTVisibility>();
        }

        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();
            Mvx.RegisterType<IDialogService, DialogService>();
            Mvx.RegisterType<IAppInformation, AppInformation>();
            Mvx.RegisterType<IStoreFeatures, StoreFeatures>();
            Mvx.RegisterType<ITileUpdateService, TileUpdateService>();
            Mvx.RegisterType<IOneDriveAuthenticator, OneDriveAuthenticator>();
            Mvx.RegisterType<IProtectedData, ProtectedData>();
            Mvx.RegisterType<ISpendingShortcut, ExpenseTile>();
            Mvx.RegisterType<IIncomeShortcut, IncomeTile>();
            Mvx.RegisterType<ITransferShortcut, TransferTile>();
            Mvx.RegisterType<INotificationService, NotificationService>();
        }

        protected override IMvxApplication CreateApp()
        {
            return new Shared.App();
        }

        protected override IMvxTrace CreateDebugTrace() => new DebugTrace();
    }
}