using MvvmCross.Core.ViewModels;
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