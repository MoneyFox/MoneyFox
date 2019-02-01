using Windows.UI.Xaml;
using MoneyFox.Foundation.Resources;

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
        ///     Update Passport
        /// </summary>
        public async void UpdatePassport()
        {
            if (await MicrosoftPassportHelper.TestPassportAvailable().ConfigureAwait(true))
            {
                PassportSwitch.IsEnabled = true;
                PassportStatus.Text = string.Empty;
            } else
            {
                PassportSwitch.IsEnabled = false;
                PassportStatus.Text = Strings.PassportErrorMessage;
            }
        }

        /// <summary>
        ///     Disables the  switch for passport.
        /// </summary>
        public void DisablePassportSwitch(object sender, RoutedEventArgs e)
        {
            UpdatePassport();
        }
    }
}