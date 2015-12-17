using System.Reflection;
using Android.App;
using Android.Content;
using Autofac;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core;
using MoneyManager.Core.ViewModels;
using MoneyManager.Localization;
using Xamarin;

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
            var insightKey = "599ff6bfdc79368ff3d5f5629a57c995fe93352e";

#if DEBUG
            insightKey = Insights.DebugModeKey;
#endif
            if (!Insights.IsInitialized)
            {
                Insights.Initialize(insightKey, Application.Context);
            }

            Strings.Culture = new Localize().GetCurrentCultureInfo();

            return new App();
        }
    }
}