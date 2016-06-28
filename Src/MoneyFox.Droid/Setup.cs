using System.Collections.Generic;
using System.Reflection;
using Android.Content;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using MoneyFox.Shared;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Resources;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.Plugins;
using MvvmCross.Plugins.Messenger;
using MoneyFox.Droid.Services;

namespace MoneyFox.Droid {
    public class Setup : MvxAndroidSetup {
        public Setup(Context applicationContext)
            : base(applicationContext) {
        }

        protected override IEnumerable<Assembly> AndroidViewAssemblies
            => new List<Assembly>(base.AndroidViewAssemblies) {
                typeof(NavigationView).Assembly,
                typeof(FloatingActionButton).Assembly,
                typeof(Toolbar).Assembly,
                typeof(DrawerLayout).Assembly,
                typeof(ViewPager).Assembly
            };

        public override void LoadPlugins(IMvxPluginManager pluginManager) {
            base.LoadPlugins(pluginManager);
            pluginManager.EnsurePluginLoaded<PluginLoader>();
        }

        protected override void InitializeFirstChance() {
            base.InitializeFirstChance();

            Mvx.RegisterType<IDialogService, DialogService>();
            Mvx.RegisterType<IOneDriveAuthenticator, OneDriveAuthenticator>();
            Mvx.RegisterType<IProtectedData, ProtectedData>();
            Mvx.RegisterType<INotificationService, NotificationService>();
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter() {
            var mvxFragmentsPresenter = new CustomPresenter(AndroidViewAssemblies);
            Mvx.RegisterSingleton<IMvxAndroidViewPresenter>(mvxFragmentsPresenter);
            return mvxFragmentsPresenter;
        }

        protected override IMvxApplication CreateApp() {
            Strings.Culture = new Localize().GetCurrentCultureInfo();

            return new App();
        }

        protected override IMvxTrace CreateDebugTrace() => new DebugTrace();
    }
}