using Android.Content;
using Autofac;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Plugins;
using MoneyManager.Core;
using MoneyManager.Core.AutoFac;
using MoneyManager.Localization;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;
using Xamarin;

namespace MoneyManager.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext)
            : base(applicationContext)
        {
        }

        public override void LoadPlugins(IMvxPluginManager pluginManager)
        {
            base.LoadPlugins(pluginManager);
            pluginManager.EnsurePluginLoaded<MvvmCross.Plugins.Messenger.PluginLoader>();
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
            var insightKey = "599ff6bfdc79368ff3d5f5629a57c995fe93352e";

#if DEBUG
            insightKey = Insights.DebugModeKey;
#endif
            if (!Insights.IsInitialized)
            {
                Insights.Initialize(insightKey, ApplicationContext);
            }

            Strings.Culture = new Localize().GetCurrentCultureInfo();

            return new App();
        }
    }
}