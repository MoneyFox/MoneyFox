using Windows.UI.Xaml;
using MoneyFox.Foundation.Resources;
using MoneyFox.Windows.Services;

namespace MoneyFox.Windows.Views.UserControls
{
    /// <summary>
    ///     View for security settings.
    /// </summary>
    public sealed partial class SettingsSecurityUserControl
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public SettingsSecurityUserControl()
        {
            InitializeComponent();
            Loaded += DisablePassportSwitch;
        }

        /// <summary>
        ///     Update Passport
        /// </summary>
        public async void UpdatePassport()
        {
            if (await MicrosoftPassportHelper.TestPassportAvailable())
            {
            }
            else
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