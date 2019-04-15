using Windows.UI.Xaml;
using MoneyFox.Foundation.Resources;
using System.Threading.Tasks;

namespace MoneyFox.Uwp.Views
{
    public sealed partial class SettingsView
    {
        public SettingsView()
        {
            InitializeComponent();
            Loaded += DisablePassportSwitch;
        }

        /// <summary>
        ///     Disables the  switch for passport.
        /// </summary>
        public async void DisablePassportSwitch(object sender, RoutedEventArgs e)
        {
            await UpdatePassport();
        }

        /// <summary>
        ///     Update Passport
        /// </summary>
        private async Task UpdatePassport()
        {
            if (await MicrosoftPassportHelper.TestPassportAvailable())
            {
                PassportSwitch.IsEnabled = true;
                PassportStatus.Text = string.Empty;
            } 
            else
            {
                PassportSwitch.IsEnabled = false;
                PassportStatus.Text = Strings.PassportErrorMessage;
            }
        }
    }
}