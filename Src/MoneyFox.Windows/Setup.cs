using Windows.UI.Xaml.Controls;
using Autofac;
using Autofac.Extras.MvvmCross;
using Cheesebaron.MvxPlugins.Connectivity;
using Cheesebaron.MvxPlugins.Connectivity.WindowsUWP;
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using Cheesebaron.MvxPlugins.Settings.WindowsUWP;
using MoneyFox.Business;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Windows.Business;
using MoneyFox.Windows.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.Plugins;
using MvvmCross.Platform.UI;
using MvvmCross.Plugins.Email;
using MvvmCross.Plugins.File;
using MvvmCross.Plugins.WebBrowser;
using PluginLoader = MvvmCross.Plugins.Messenger.PluginLoader;
using MvvmCross.Platform.IoC;
using MvvmCross.Plugins.Email.Uwp;
using MvvmCross.Plugins.File.Uwp;
using MvvmCross.Plugins.Visibility.Uwp;
using MvvmCross.Plugins.WebBrowser.Uwp;
using MvvmCross.Uwp.Platform;

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
            Mvx.RegisterType<IMvxFileStore, MvxWindowsCommonFileStore>();
            Mvx.RegisterType<ISettings, WindowsUwpSettings>();
            Mvx.RegisterType<IConnectivity, Connectivity>();
            Mvx.RegisterType<IMvxNativeVisibility, MvxWinRTVisibility>();
        }

        protected override IMvxIoCProvider CreateIocProvider()
        {
            var cb = new ContainerBuilder();

            cb.RegisterModule<BusinessModule>();
            cb.RegisterModule<WindowsModule>();

            return new AutofacMvxIocProvider(cb.Build());
        }

        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();
            Mvx.RegisterType<IDialogService, DialogService>();
            Mvx.RegisterType<IModifyDialogService, ModifyDialogService>();
            Mvx.RegisterType<ITileUpdateService, TileUpdateService>();
            Mvx.RegisterType<IOneDriveAuthenticator, OneDriveAuthenticator>();
            Mvx.RegisterType<IProtectedData, ProtectedData>();
            Mvx.RegisterType<INotificationService, NotificationService>();
            Mvx.RegisterType<IBackgroundTaskManager, BackgroundTaskManager>();
            Mvx.RegisterType<ITileManager, TileManager>();
        }

        protected override IMvxApplication CreateApp() => new MoneyFox.Business.App();

        protected override IMvxTrace CreateDebugTrace() => new DebugTrace();
    }
}