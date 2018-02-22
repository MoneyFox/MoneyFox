using Android.Content;
using Android.Widget;
using Autofac;
using Autofac.Extras.MvvmCross;
using Clans.Fab;
using MoneyFox.Business;
using MoneyFox.Droid.CustomBinding;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;
using MvvmCross.Platform.Plugins;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views;
using MvvmCross.Localization;
using MvvmCross.Platform.Converters;
using MvvmCross.Platform.IoC;
using PluginLoader = MvvmCross.Plugins.Email.PluginLoader;

namespace MoneyFox.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext)
            : base(applicationContext)
        {
        }

        /// <inheritdoc />
        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            MvxAppCompatSetupHelper.FillTargetFactories(registry);
            registry.RegisterFactory(new MvxCustomBindingFactory<LinearLayout>("WarningBackgroundShape", (view) => new WarningBackgroundShapeBinding(view)));
            registry.RegisterFactory(new MvxCustomBindingFactory<FloatingActionButton>("Click", (view) => new FloatingActionButtonClickBinding(view)));
            base.FillTargetFactories(registry);
        }

        /// <inheritdoc />
        public override void LoadPlugins(IMvxPluginManager pluginManager)
        {
            base.LoadPlugins(pluginManager);
            pluginManager.EnsurePluginLoaded<PluginLoader>();
        }

        /// <inheritdoc />
        protected override IMvxIoCProvider CreateIocProvider()
        {
            var cb = new ContainerBuilder();
            
            cb.RegisterModule<DroidModule>();

            return new AutofacMvxIocProvider(cb.Build());
        }

        /// <inheritdoc />
        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            return new MvxAppCompatViewPresenter(AndroidViewAssemblies);
        }

        /// <inheritdoc />
        protected override void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);
            registry.AddOrOverwrite("Language", new MvxLanguageConverter());
        }

        /// <inheritdoc />
        protected override IMvxApplication CreateApp()
        {
            Strings.Culture = new Localize().GetCurrentCultureInfo();
            return new App();
        }
    }
}