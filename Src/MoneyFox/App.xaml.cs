using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using PCLAppConfig;
using PCLAppConfig.FileSystemStream;
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

            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            ConfigurationManager.Initialise(PortableStream.Current);
            var iosAppCenterSecret = ConfigurationManager.AppSettings["IosAppcenterSecret"];
            var androidAppCenterSecret = ConfigurationManager.AppSettings["AndroidAppcenterSecret"];

            AppCenter.Start($"android={androidAppCenterSecret};" +
                            $"ios={iosAppCenterSecret}",
                            typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
