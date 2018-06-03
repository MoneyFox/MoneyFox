using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Windows.Business;
using MoneyFox.Windows.Services;
using MvvmCross.Platforms.Uap.Core;
using MvvmCross.Plugin;
using MvvmCross.Plugin.Email;
using MvvmCross.Plugin.Email.Platforms.Uap;
using MvvmCross.Plugin.File;
using MvvmCross.Plugin.File.Platforms.Uap;
using MvvmCross.Plugin.Messenger;
using MvvmCross.Plugin.Visibility.Platforms.Uap;
using MvvmCross.Plugin.WebBrowser;
using MvvmCross.Plugin.WebBrowser.Platforms.Uap;
using MvvmCross.UI;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using ISettings = MoneyFox.Business.ISettings;
using Mvx = MvvmCross.Mvx;

namespace MoneyFox.Windows
{
    public class Setup : MvxWindowsSetup<CoreApp>
    {
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            Mvx.LazyConstructAndRegisterSingleton<IConnectivity, ConnectivityImplementation>();
            Mvx.LazyConstructAndRegisterSingleton<IDialogService, DialogService>();
            Mvx.LazyConstructAndRegisterSingleton<IModifyDialogService, ModifyDialogService>();
            Mvx.LazyConstructAndRegisterSingleton<IOneDriveAuthenticator, OneDriveAuthenticator>();
            Mvx.LazyConstructAndRegisterSingleton<IProtectedData, ProtectedData>();
            Mvx.LazyConstructAndRegisterSingleton<INotificationService, NotificationService>();
            Mvx.LazyConstructAndRegisterSingleton<ITileManager, TileManager>();
            Mvx.LazyConstructAndRegisterSingleton<IAppInformation, WindowsAppInformation>();
            Mvx.LazyConstructAndRegisterSingleton<IStoreOperations, MarketplaceOperations>();
            Mvx.LazyConstructAndRegisterSingleton<ISettings, Settings>();
            Mvx.LazyConstructAndRegisterSingleton<IBackgroundTaskManager, BackgroundTaskManager>();

            DependencyRegistrator.RegisterDependencies();
        }

        /// <inheritdoc />
        public override void LoadPlugins(IMvxPluginManager pluginManager)
        {
            //We have to do this here, since the loading via bootloader won't work for UWP projects
            Mvx.LazyConstructAndRegisterSingleton<IMvxComposeEmailTask, MvxComposeEmailTask>();
            Mvx.LazyConstructAndRegisterSingleton<IMvxWebBrowserTask, MvxWebBrowserTask>();
            Mvx.LazyConstructAndRegisterSingleton<IMvxFileStore, MvxWindowsFileStore>();
            Mvx.LazyConstructAndRegisterSingleton<IMvxNativeVisibility, MvxWinRTVisibility>();
            Mvx.LazyConstructAndRegisterSingleton<IMvxMessenger, MvxMessengerHub>();
        }

        public override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            var result = GetViewAssemblies();
            var assemblyList = result.ToList();
            assemblyList.Add(typeof(MainViewModel).Assembly);
            return assemblyList;
        }
    }
}