using Beezy.MvvmCross.Plugins.SecureStorage;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core.Manager;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Localization;

namespace MoneyManager.Core.ViewModels
{
    public class EnterPasswordViewModel : BaseViewModel
    {
        private readonly PasswordManager passwordManager;
        private readonly IDialogService dialogService;

        public EnterPasswordViewModel(PasswordManager passwordManager, IDialogService dialogService)
        {
            this.passwordManager = passwordManager;
            this.dialogService = dialogService;
        }

        /// <summary>
        ///     The password the user entered
        /// </summary>
        public string Password { get; set; }

        public MvxCommand LoginCommand => new MvxCommand(Login);

        private void Login()
        {
            if (!passwordManager.ValidatePassword(Password))
            {
                dialogService.ShowMessage(Strings.PasswordWrongTitle, Strings.PasswordWrongMessage);
                return;
            }

            ShowViewModel<MainViewModel>();
        }
    }
}
