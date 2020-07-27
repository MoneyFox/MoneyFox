using Xamarin.Forms;


namespace MoneyFox
{
    public partial class App : Xamarin.Forms.Application
    {
        public App()
        {
            Device.SetFlags(new string[] {
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
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
