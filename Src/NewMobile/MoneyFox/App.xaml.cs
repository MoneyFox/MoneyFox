using Xamarin.Forms;


namespace MoneyFox
{
    public partial class App : Xamarin.Forms.Application
    {

        public App()
        {
            Device.SetFlags(new string[] {
                "AppTheme_Experimental"
            });

            App.Current.UserAppTheme = OSAppTheme.Light;

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
