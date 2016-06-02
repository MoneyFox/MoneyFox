using MvvmCross.Core.ViewModels;

namespace MoneyFox.Shared.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public MvxCommand LoginNavigationCommand => new MvxCommand(LoginNavigation);

        private void LoginNavigation()
        {
            ShowViewModel<MainViewModel>();
        }
    }
}