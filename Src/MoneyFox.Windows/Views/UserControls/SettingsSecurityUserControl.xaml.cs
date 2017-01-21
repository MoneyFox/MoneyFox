using Windows.UI.Xaml;
using MoneyFox.Windows.Services;

namespace MoneyFox.Windows.Views.UserControls
{
    public sealed partial class SettingsSecurityUserControl
    {
        private readonly AppShell appShell;


        public  async void UpdatePassport()
        {

            if (await MicrosoftPassportHelper.testPassportAvailable())
            {

            }else
            {
                PassportSwitch.IsEnabled = false;
                PassportStatus.Text = " Mircrosoft Passport is not enabled on this device.\nPlease go to settings and enable it!";
            }
        }

        public void DisablePassportSwitch(object sender, RoutedEventArgs e)
        {
            UpdatePassport();
        }

        public SettingsSecurityUserControl()
        {
            InitializeComponent();
            appShell = Window.Current.Content as AppShell;
            Loaded += new RoutedEventHandler(DisablePassportSwitch);
        }


   }

}