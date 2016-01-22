using Windows.UI.Xaml.Controls;
using Autofac;
using Beezy.MvvmCross.Plugins.SecureStorage;
using Beezy.MvvmCross.Plugins.SecureStorage.WindowsStore;
using MoneyManager.Core;
using MoneyManager.Core.AutoFac;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
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
            Mvx.RegisterType<IMvxProtectedData, MvxStoreProtectedData>();
            Mvx.RegisterType<IMvxFileStore, MvxWindowsCommonFileStore>();
        }

        protected override IMvxIoCProvider CreateIocProvider()
        {
            var cb = new ContainerBuilder();

            cb.RegisterModule<CoreModule>();
            cb.RegisterModule<WindowsModule>();

            return new AutofacMvxIocProvider(cb.Build());
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