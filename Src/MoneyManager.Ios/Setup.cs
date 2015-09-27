using Cirrious.CrossCore;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Localization;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinIOS;
using UIKit;

namespace MoneyManager.Ios
{
    public class Setup : MvxTouchSetup
    {
        public Setup(IMvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }

        public Setup(IMvxApplicationDelegate applicationDelegate, IMvxTouchViewPresenter presenter)
            : base(applicationDelegate, presenter)
        {
        }

        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            Mvx.RegisterType<ISQLitePlatform, SQLitePlatformIOS>();
            Mvx.RegisterType<IDatabasePath, DatabasePath>();
            Mvx.RegisterType<IDialogService, DialogService>();
            Mvx.RegisterType<IAppInformation, AppInformation>();
            Mvx.RegisterType<IStoreFeatures, StoreFeatures>();
        }

        protected override IMvxApplication CreateApp()
        {
            Strings.Culture = new Localize().GetCurrentCultureInfo();

            return new App();
        }
    }
}