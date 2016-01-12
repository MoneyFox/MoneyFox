using AI.XamarinSDK.Abstractions;
using Android.Content;
using Autofac;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core;
using MoneyManager.Core.AutoFac;
using MoneyManager.Localization;

namespace MoneyManager.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext)
            : base(applicationContext)
        {
        }

        protected override IMvxIoCProvider CreateIocProvider()
        {
            var cb = new ContainerBuilder();

            cb.RegisterModule<CoreModule>();
            cb.RegisterModule<AndroidModule>();

            return new AutofacMvxIocProvider(cb.Build());
        }

        protected override IMvxApplication CreateApp()
        {
            //ApplicationInsights.Setup("ac915a37-36f5-436a-b85b-5a5617838bc8");
            //ApplicationInsights.Start();

            Strings.Culture = new Localize().GetCurrentCultureInfo();

            return new App();
        }
    }
}