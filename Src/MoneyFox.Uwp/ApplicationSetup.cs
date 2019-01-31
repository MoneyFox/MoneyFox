using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MoneyFox.BusinessLogic.Backup;
using MoneyFox.Presentation;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.ViewModels;
using MoneyFox.Uwp.Business;
using MvvmCross.IoC;
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
using Mvx = MvvmCross.Mvx;

namespace MoneyFox.Uwp
{
    public class ApplicationSetup : MvxWindowsSetup<CoreApp>
    {
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IDialogService, DialogService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IOneDriveAuthenticator, OneDriveAuthenticator>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IProtectedData, ProtectedData>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IAppInformation, WindowsAppInformation>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IStoreOperations, MarketplaceOperations>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IBackgroundTaskManager, BackgroundTaskManager>();
        }

        /// <inheritdoc />
        public override void LoadPlugins(IMvxPluginManager pluginManager)
        {
            //We have to do this here, since the loading via bootloader won't work for UWP projects
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxComposeEmailTask, MvxComposeEmailTask>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxWebBrowserTask, MvxWebBrowserTask>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxFileStore, MvxWindowsFileStore>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxNativeVisibility, MvxWinRTVisibility>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxMessenger, MvxMessengerHub>();
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