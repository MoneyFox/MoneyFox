using MoneyFox.Business;
using MoneyFox.Droid.Manager;
using MoneyFox.Droid.OneDriveAuth;
using MoneyFox.Droid.Services;
using MoneyFox.Droid.Src;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MvvmCross;
using MvvmCross.Forms.Platforms.Android.Core;
using MvvmCross.Forms.Presenters;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Plugin.SecureStorage;

namespace MoneyFox.Droid
{
    public class Setup : MvxFormsAndroidSetup<CoreApp, App>
    {
        public Setup()
        {
            Strings.Culture = new Localize().GetCurrentCultureInfo();
        }

        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            SecureStorageImplementation.StorageType = StorageTypes.AndroidKeyStore;

            Mvx.LazyConstructAndRegisterSingleton<IConnectivity, ConnectivityImplementation>();
            Mvx.LazyConstructAndRegisterSingleton<IDialogService, DialogService>();
            Mvx.LazyConstructAndRegisterSingleton<IOneDriveAuthenticator, OneDriveAuthenticator>();
            Mvx.LazyConstructAndRegisterSingleton<IProtectedData, ProtectedData>();
            Mvx.LazyConstructAndRegisterSingleton<IAppInformation, DroidAppInformation>();
            Mvx.LazyConstructAndRegisterSingleton<IStoreOperations, PlayStoreOperations>();
            Mvx.LazyConstructAndRegisterSingleton<ISettings, Settings>();
            Mvx.LazyConstructAndRegisterSingleton<IBackgroundTaskManager, BackgroundTaskManager>();

            DependencyRegistrator.RegisterDependencies();
        }

        protected override IMvxFormsPagePresenter CreateFormsPagePresenter(IMvxFormsViewPresenter viewPresenter)
        {
            var formsPresenter = base.CreateFormsPagePresenter(viewPresenter);
            Mvx.RegisterSingleton(formsPresenter);
            return formsPresenter;
        }

        //public override MvxLogProviderType GetDefaultLogProviderType()
        //    => MvxLogProviderType.Serilog;

        //protected override IMvxLogProvider CreateLogProvider()
        //{
        //    Log.Logger = new LoggerConfiguration()
        //                 .MinimumLevel.Debug()
        //                 .WriteTo.AndroidLog()
        //                 .CreateLogger();
        //    return base.CreateLogProvider();
        //}
    }
}