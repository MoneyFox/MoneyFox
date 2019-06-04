using MoneyFox.Presentation;
using MoneyFox.ServiceLayer.Interfaces;
using MvvmCross;
using MvvmCross.Forms.Platforms.Ios.Core;
using MvvmCross.IoC;

namespace MoneyFox.iOS
{
    public class ApplicationSetup : MvxFormsIosSetup<CoreApp, App>
    {
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IDialogService, DialogService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IAppInformation, AppInformation>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IStoreOperations, StoreOperations>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IBackgroundTaskManager, BackgroundTaskManager>();
        }
    }
}