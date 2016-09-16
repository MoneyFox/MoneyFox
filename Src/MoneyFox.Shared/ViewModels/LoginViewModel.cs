using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;

namespace MoneyFox.Shared.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        /// <summary>
        ///     Provides an TextSource for the translation binding on this page.
        /// </summary>
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("", GetType().Name);

        /// <summary>
        ///     Navigates after the login was successful
        /// </summary>
        public MvxCommand LoginNavigationCommand => new MvxCommand(LoginNavigation);

        private void LoginNavigation()
        {
            ShowViewModel<MainViewModel>();
        }
    }
}