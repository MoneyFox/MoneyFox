using MoneyFox.Business;
using MoneyFox.Foundation.Interfaces;
using MvvmCross;
using MvvmCross.Forms.Platforms.Ios.Core;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;

namespace MoneyFox.iOS
{
    public class Setup : MvxFormsIosSetup<CoreApp, App>
    {
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            Mvx.LazyConstructAndRegisterSingleton<IConnectivity, ConnectivityImplementation>();
            Mvx.LazyConstructAndRegisterSingleton<IDialogService, DialogService>();
            //Mvx.LazyConstructAndRegisterSingleton<IOneDriveAuthenticator, OneDriveAuthenticator>();
            Mvx.LazyConstructAndRegisterSingleton<IProtectedData, ProtectedData>();
            Mvx.LazyConstructAndRegisterSingleton<ITileManager, TileManager>();
            Mvx.LazyConstructAndRegisterSingleton<IAppInformation, AppInformation>();
            Mvx.LazyConstructAndRegisterSingleton<IStoreOperations, StoreOperations>();
            Mvx.LazyConstructAndRegisterSingleton<ISettings, Settings>();

            DependencyRegistrator.RegisterDependencies();
        }
    }
}