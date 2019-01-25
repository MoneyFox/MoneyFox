using MoneyFox.Foundation.Interfaces;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace MoneyFox.ServiceLayer.ViewModels
{
    /// <summary>
    ///     Representation of the LoginView.
    /// </summary>
    public class LoginViewModel : BaseNavigationViewModel
    {
        private readonly ISettingsManager settingsManager;
        private readonly IMvxNavigationService navigationService;

        /// <summary>
        ///     Constructor
        /// </summary>
        public LoginViewModel(ISettingsManager settingsManager,
                              IMvxLogProvider logProvider,
                              IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            this.settingsManager = settingsManager;
            this.navigationService = navigationService;
        }

        /// <summary>
        ///     Navigates after the login was successful
        /// </summary>
        public MvxCommand LoginNavigationCommand => new MvxCommand(LoginNavigation);


        public bool PasswordEnabled => settingsManager.PasswordRequired;

        public bool PassportEnabled => settingsManager.PassportEnabled;

        private void LoginNavigation()
        {
            navigationService.Navigate<MainViewModel>();

            var mainView = Mvx.IoCProvider.Resolve<MainViewModel>();
            mainView?.ShowAccountListCommand.ExecuteAsync();
        }
    }
}