using Windows.UI.Xaml;
using MoneyFox.Foundation.Resources;
using MoneyFox.Windows.Services;
using Windows.UI.Xaml.Controls;

namespace MoneyFox.Windows.Views.UserControls
{
    /// <summary>
    ///     View for security settings.
    /// </summary>
    public sealed partial class SettingsSecurityUserControl
    {

        public PasswordBox Password;
        public PasswordBox PasswordConfirmation;
        /// <summary>
        ///     Constructor
        /// </summary>
        public SettingsSecurityUserControl()
        {
            InitializeComponent();
            Loaded += DisablePassportSwitch;
            Password = password;
            PasswordConfirmation = passwordConfirmation;
        }

        /// <summary>
        ///     Update Passport
        /// </summary>
        public async void UpdatePassport()
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

        /// <summary>
        ///     Disables the  switch for passport.
        /// </summary>
        public void DisablePassportSwitch(object sender, RoutedEventArgs e)
        {
            UpdatePassport();
        }
    }
}