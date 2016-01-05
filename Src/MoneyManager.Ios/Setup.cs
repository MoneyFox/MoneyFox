using Autofac;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core;
using MoneyManager.Core.AutoFac;
using MoneyManager.Localization;
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

        protected override IMvxIoCProvider CreateIocProvider()
        {
            var cb = new ContainerBuilder();

            cb.RegisterModule<CoreModule>();
            cb.RegisterModule<IosModule>();

            return new AutofacMvxIocProvider(cb.Build());
        }


        protected override IMvxApplication CreateApp()
        {
            Strings.Culture = new Localize().GetCurrentCultureInfo();

            return new App();
        }
    }
}