using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Resources;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Shared.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IPasswordStorage passwordStorage;
        private readonly IDialogService dialogService;

        public LoginViewModel(IPasswordStorage passwordStorage, IDialogService dialogService)
        {
            this.passwordStorage = passwordStorage;
            this.dialogService = dialogService;
        }

        public MvxCommand<string> LoginCommand => new MvxCommand<string>(Login);

        private void Login(string password)
        {
            if (!passwordStorage.ValidatePassword(password))
            {
                dialogService.ShowMessage(Strings.PasswordWrongTitle, Strings.PasswordWrongMessage);
                return;
            }

            ShowViewModel<MainViewModel>();
        }
    }
}
