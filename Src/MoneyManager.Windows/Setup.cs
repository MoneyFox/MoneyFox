using Windows.UI.Xaml.Controls;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Interfaces.Shotcuts;
using MoneyManager.Windows.Services;
using MoneyManager.Windows.Shortcut;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;
using MvvmCross.Plugins.Email;
using MvvmCross.Plugins.Email.WindowsCommon;
using MvvmCross.Plugins.File;
using MvvmCross.Plugins.File.WindowsCommon;
using MvvmCross.Plugins.Sqlite;
using MvvmCross.Plugins.Sqlite.WindowsUWP;
using MvvmCross.Plugins.WebBrowser;
using MvvmCross.Plugins.WebBrowser.WindowsCommon;
using MvvmCross.WindowsUWP.Platform;
using Xamarin;

namespace MoneyManager.Windows
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
            pluginManager.EnsurePluginLoaded<MvvmCross.Plugins.Messenger.PluginLoader>();

            //We have to do this here, since the loading via bootloader won't work for UWP projects
            Mvx.RegisterType<IMvxComposeEmailTask, MvxComposeEmailTask>();
            Mvx.RegisterType<IMvxWebBrowserTask, MvxWebBrowserTask>();
            Mvx.RegisterType<IMvxSqliteConnectionFactory, WindowsSqliteConnectionFactory>();
            Mvx.RegisterType<IMvxFileStore, MvxWindowsCommonFileStore>();
        }

        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            Mvx.RegisterType<IDialogService, DialogService>();
            Mvx.RegisterType<IAppInformation, AppInformation>();
            Mvx.RegisterType<IStoreFeatures, StoreFeatures>();
            Mvx.RegisterType<IRoamingSettings, RoamingSettings>();
            Mvx.RegisterType<ILocalSettings, LocalSettings>();
            Mvx.RegisterType<IUserNotification, UserNotification>();
            Mvx.RegisterType<IOneDriveAuthenticator, OneDriveAuthenticator>();
            Mvx.RegisterType<IProtectedData, ProtectedData>();
            Mvx.RegisterType<ISpendingShortcut, ExpenseTile>();
            Mvx.RegisterType<IIncomeShortcut, IncomeTile>();
            Mvx.RegisterType<ITransferShortcut, TransferTile>();
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