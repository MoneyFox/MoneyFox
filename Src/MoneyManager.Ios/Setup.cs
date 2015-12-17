using System.Reflection;
using Autofac;
using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core;
using MoneyManager.Core.ViewModels;
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
            Mvx.RegisterType<ILocalSettings, LocalSettings>();
        }

        protected override IMvxIoCProvider CreateIocProvider()
        {
            var cb = new ContainerBuilder();

            cb.RegisterModule<CoreModule>();
            cb.RegisterModule<IosModule>();

            // This is an important step that ensures all the ViewModel's are loaded into the container.
            // Without this, it was observed that MvvmCross wouldn't register them by itself; needs more investigation.
            cb.RegisterAssemblyTypes(typeof(MainViewModel).GetTypeInfo().Assembly)
                .AssignableTo<MvxViewModel>()
                .As<IMvxViewModel, MvxViewModel>()
                .AsSelf();

            return new AutofacMvxIocProvider(cb.Build());
        }


        protected override IMvxApplication CreateApp()
        {
            Strings.Culture = new Localize().GetCurrentCultureInfo();

            return new App();
        }
    }
}