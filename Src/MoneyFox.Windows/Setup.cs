using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Windows.Business;
using MoneyFox.Windows.Services;
using MvvmCross.Platforms.Uap.Core;
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

            MvvmCross.Mvx.ConstructAndRegisterSingleton<IConnectivity, ConnectivityImplementation>();
            MvvmCross.Mvx.ConstructAndRegisterSingleton<IDialogService, DialogService>();
            MvvmCross.Mvx.ConstructAndRegisterSingleton<IModifyDialogService, ModifyDialogService>();
            MvvmCross.Mvx.ConstructAndRegisterSingleton<IOneDriveAuthenticator, OneDriveAuthenticator>();
            MvvmCross.Mvx.ConstructAndRegisterSingleton<IProtectedData, ProtectedData>();
            MvvmCross.Mvx.ConstructAndRegisterSingleton<INotificationService, NotificationService>();
            MvvmCross.Mvx.ConstructAndRegisterSingleton<ITileManager, TileManager>();
            MvvmCross.Mvx.ConstructAndRegisterSingleton<IAppInformation, WindowsAppInformation>();
            MvvmCross.Mvx.ConstructAndRegisterSingleton<IStoreOperations, MarketplaceOperations>();
            Mvx.ConstructAndRegisterSingleton<ISettings, Settings>();
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