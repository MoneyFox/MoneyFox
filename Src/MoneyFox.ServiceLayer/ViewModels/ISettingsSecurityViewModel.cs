using MvvmCross.Commands;
using MvvmCross.Localization;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public interface ISettingsSecurityViewModel
    {
        /// <summary>
        ///     Grants the GUI access to the password setting.
        /// </summary>
        bool IsPasswortActive { get; set; }

        bool IsPassportActive { get; set; }

        /// <summary>
        ///     Provides an TextSource for the translation binding on this page.
        /// </summary>
        IMvxLanguageBinder TextSource { get; }

        /// <summary>
        ///     The password that the user set.
        /// </summary>
        string Password { get; set; }

        /// <summary>
        ///     The password confirmation the user entered.
        /// </summary>
        string PasswordConfirmation { get; set; }

        /// <summary>
        ///     Save the password to the secure storage of the current platform
        /// </summary>
        MvxCommand SavePasswordCommand { get; }

        /// <summary>
        ///     Loads the password from the secure storage
        /// </summary>
        MvxCommand LoadCommand { get; }

        /// <summary>
        ///     Remove the password from the secure storage
        /// </summary>
        MvxCommand UnloadCommand { get; }
    }
}