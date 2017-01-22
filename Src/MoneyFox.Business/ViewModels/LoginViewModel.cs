using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;
using MoneyFox.Foundation.Interfaces;

namespace MoneyFox.Business.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {

        private readonly ISettingsManager settingsManager;
        private readonly IDialogService dialogService;
        private readonly IPasswordStorage passwordStorage;

        public LoginViewModel(ISettingsManager settingsManager, IPasswordStorage passwordStorage, IDialogService dialogService)
        {
            this.settingsManager = settingsManager;
            this.passwordStorage = passwordStorage;
            this.dialogService = dialogService;
        }


        /// <summary>
        ///     Provides an TextSource for the translation binding on this page.
        /// </summary>
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("", GetType().Name);

        /// <summary>
        ///     Navigates after the login was successful
        /// </summary>
        public MvxCommand LoginNavigationCommand => new MvxCommand(LoginNavigation);


        public bool PasswordEnabled
        {
            get { return settingsManager.PasswordRequired; }
        }

        public bool PassportEnabled
        {
            get { return settingsManager.PassportEnabled; }
        }

        private void LoginNavigation()
        {
            ShowViewModel<MainViewModel>();
            ShowViewModel<AccountListViewModel>();
            ShowViewModel<MenuViewModel>();
        }
    }
}