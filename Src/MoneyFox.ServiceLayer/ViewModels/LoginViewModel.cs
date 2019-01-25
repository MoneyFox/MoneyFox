using MoneyFox.ServiceLayer.Facades;
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
        private readonly ISettingsFacade settingsFacade;
        private readonly IMvxNavigationService navigationService;

        /// <summary>
        ///     Constructor
        /// </summary>
        public LoginViewModel(ISettingsFacade settingsFacade,
                              IMvxLogProvider logProvider,
                              IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            this.settingsFacade = settingsFacade;
            this.navigationService = navigationService;
        }

        /// <summary>
        ///     Navigates after the login was successful
        /// </summary>
        public MvxCommand LoginNavigationCommand => new MvxCommand(LoginNavigation);


        public bool PasswordEnabled => settingsFacade.PasswordRequired;

        public bool PassportEnabled => settingsFacade.PassportEnabled;

        private void LoginNavigation()
        {
            navigationService.Navigate<MainViewModel>();

            var mainView = Mvx.IoCProvider.Resolve<MainViewModel>();
            mainView?.ShowAccountListCommand.ExecuteAsync();
        }
    }
}