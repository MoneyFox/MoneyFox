using Xamarin.Forms;
using MoneyFox.Services;


namespace MoneyFox
{
    public partial class App : Application
    {

        public App()
        {
            Device.SetFlags(new string[] {
                "AppTheme_Experimental"
            });

            App.Current.UserAppTheme = OSAppTheme.Light;

            InitializeComponent();

            DependencyService.Register<MockDataStore>();
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
