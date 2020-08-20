using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MoneyFox.Application;
using MoneyFox.Application.Common.Adapters;
using MoneyFox.Application.Common.Facades;
using PCLAppConfig;
using PCLAppConfig.FileSystemStream;
using System.Globalization;
using Xamarin.Forms;


namespace MoneyFox
{
    public partial class App : Xamarin.Forms.Application
    {
        public App()
        {
            Xamarin.Forms.Device.SetFlags(new [] {
                "AppTheme_Experimental",
                "SwipeView_Experimental"
            });

            App.Current.UserAppTheme = App.Current.RequestedTheme != OSAppTheme.Unspecified
                ? App.Current.RequestedTheme
                : OSAppTheme.Dark;

            App.Current.RequestedThemeChanged += (s, a) =>
            {
                App.Current.UserAppTheme = a.RequestedTheme;
            };

            CultureHelper.CurrentCulture = new CultureInfo(new SettingsFacade(new SettingsAdapter()).DefaultCulture);

            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            if(ConfigurationManager.AppSettings == null)
            {
                ConfigurationManager.Initialise(PortableStream.Current);
            }

            InitializeAppCenter();
        }

        private static void InitializeAppCenter()
        {
            if(ConfigurationManager.AppSettings != null)
            {
                var iosAppCenterSecret = ConfigurationManager.AppSettings["IosAppcenterSecret"];
                var androidAppCenterSecret = ConfigurationManager.AppSettings["AndroidAppcenterSecret"];

                AppCenter.Start($"android={androidAppCenterSecret};" +
                                $"ios={iosAppCenterSecret}",
                                typeof(Analytics), typeof(Crashes));
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
