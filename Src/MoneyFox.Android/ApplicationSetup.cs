using MoneyFox.Droid.Manager;
using MoneyFox.Droid.Services;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation;
using MoneyFox.ServiceLayer.Interfaces;
using MvvmCross;
using MvvmCross.Forms.Platforms.Android.Core;
using MvvmCross.Forms.Presenters;
using MvvmCross.IoC;
using Plugin.SecureStorage;

namespace MoneyFox.Droid
{
    public class ApplicationSetup : MvxFormsAndroidSetup<CoreApp, App>
    {
        public ApplicationSetup() => Strings.Culture = new Localize().GetCurrentCultureInfo();

        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            SecureStorageImplementation.StorageType = StorageTypes.AndroidKeyStore;

            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IDialogService, DialogService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IProtectedData, ProtectedData>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IAppInformation, DroidAppInformation>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IStoreOperations, PlayStoreOperations>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IBackgroundTaskManager, BackgroundTaskManager>();
        }

        protected override IMvxFormsPagePresenter CreateFormsPagePresenter(IMvxFormsViewPresenter viewPresenter)
        {
            var formsPresenter = base.CreateFormsPagePresenter(viewPresenter);
            Mvx.IoCProvider.RegisterSingleton(formsPresenter);
            return formsPresenter;
        }
    }
}