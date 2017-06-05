using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;
using MoneyFox.Foundation.Interfaces;

namespace MoneyFox.Business.ViewModels
{
    /// <summary>
    ///     Representation of the LoginView.
    /// </summary>
    public class LoginViewModel : MvxViewModel
    {
        private readonly ISettingsManager settingsManager;

        /// <summary>
        ///     Constructor
        /// </summary>
        public LoginViewModel(ISettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
        }


        /// <summary>
        ///     Provides an TextSource for the translation binding on this page.
        /// </summary>
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("", GetType().Name);

        /// <summary>
        ///     Navigates after the login was successful
        /// </summary>
        public MvxCommand LoginNavigationCommand => new MvxCommand(LoginNavigation);


        public bool PasswordEnabled => settingsManager.PasswordRequired;

        public bool PassportEnabled => settingsManager.PassportEnabled;

        private void LoginNavigation()
        {
            ShowViewModel<MainViewModel>();
            ShowViewModel<AccountListViewModel>();
            ShowViewModel<MenuViewModel>();
        }
    }
}