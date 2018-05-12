using MvvmCross.Localization;
using MoneyFox.Foundation.Interfaces;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace MoneyFox.Business.ViewModels
{
    /// <summary>
    ///     Representation of the LoginView.
    /// </summary>
    public class LoginViewModel : BaseViewModel
    {
        private readonly ISettingsManager settingsManager;
        private readonly IMvxNavigationService navigationService;

        /// <summary>
        ///     Constructor
        /// </summary>
        public LoginViewModel(ISettingsManager settingsManager, IMvxNavigationService navigationService)
        {
            this.settingsManager = settingsManager;
            this.navigationService = navigationService;
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
            navigationService.Navigate<MainViewModel>();
            navigationService.Navigate<AccountListViewModel>();
            navigationService.Navigate<MenuViewModel>();
        }
    }
}