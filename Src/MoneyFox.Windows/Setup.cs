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

            Mvx.ConstructAndRegisterSingleton<IConnectivity, ConnectivityImplementation>();
            Mvx.ConstructAndRegisterSingleton<IDialogService, DialogService>();
            Mvx.ConstructAndRegisterSingleton<IModifyDialogService, ModifyDialogService>();
            Mvx.ConstructAndRegisterSingleton<IOneDriveAuthenticator, OneDriveAuthenticator>();
            Mvx.ConstructAndRegisterSingleton<IProtectedData, ProtectedData>();
            Mvx.ConstructAndRegisterSingleton<INotificationService, NotificationService>();
            Mvx.ConstructAndRegisterSingleton<ITileManager, TileManager>();
            Mvx.ConstructAndRegisterSingleton<IAppInformation, WindowsAppInformation>();
            Mvx.ConstructAndRegisterSingleton<IStoreOperations, MarketplaceOperations>();
            Mvx.ConstructAndRegisterSingleton<ISettings, Settings>();
            Mvx.ConstructAndRegisterSingleton<IBackgroundTaskManager, BackgroundTaskManager>();

            DependencyRegistrator.RegisterDependencies();
        }

        /// <inheritdoc />

        public override void LoadPlugins(IMvxPluginManager pluginManager)
        {
            base.LoadPlugins(pluginManager);

            //We have to do this here, since the loading via bootloader won't work for UWP projects
            Mvx.RegisterType<IMvxComposeEmailTask, MvxComposeEmailTask>();
            Mvx.RegisterType<IMvxWebBrowserTask, MvxWebBrowserTask>();
            Mvx.RegisterType<IMvxFileStore, MvxWindowsFileStore>();
            Mvx.RegisterType<IMvxNativeVisibility, MvxWinRTVisibility>();
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