using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core.Authentication;
using MoneyManager.Core.Manager;
using MoneyManager.DataAccess;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Localization;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class PasswordUserControlViewModel : BaseViewModel
    {
        private const string PASSWORD_KEY = "password";
        private readonly IDialogService dialogService;
        private readonly PasswordStorage passwordStorage;
        private readonly SettingDataAccess settings;

        public PasswordUserControlViewModel(SettingDataAccess settings,
            PasswordStorage passwordStorage, IDialogService dialogService)
        {
            this.settings = settings;
            this.passwordStorage = passwordStorage;
            this.dialogService = dialogService;
        }

        /// <summary>
        ///     Grants the GUI access to the password setting.
        /// </summary>
        public bool IsPasswortActive
        {
            get { return settings.PasswordRequired; }
            set { settings.PasswordRequired = value; }
        }

        /// <summary>
        ///     The password who the user set.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     The password confirmation the user entered.
        /// </summary>
        public string PasswordConfirmation { get; set; }

        /// <summary>
        ///     Save the password to the secure storage of the current platform
        /// </summary>
        public MvxCommand SavePasswordCommand => new MvxCommand(SavePassword);

        /// <summary>
        ///     Loads the password from the secure storage
        /// </summary>
        public MvxCommand LoadCommand => new MvxCommand(LoadData);

        /// <summary>
        ///     Remove the password from the secure storage
        /// </summary>
        public MvxCommand UnloadedCommand => new MvxCommand(RemovePassword);

        private void SavePassword()
        {
            if (Password != PasswordConfirmation)
            {
                dialogService.ShowMessage(Strings.PasswordConfirmationWrongTitle,
                    Strings.PasswordConfirmationWrongMessage);
                return;
            }

            dialogService.ShowMessage(Strings.PasswordSavedTitle, Strings.PasswordSavedMessage);
        }

        private void LoadData()
        {
            if (IsPasswortActive)
            {
                Password = passwordStorage.LoadPassword();
                PasswordConfirmation = passwordStorage.LoadPassword();
            }
        }

        private void RemovePassword()
        {
            passwordStorage.RemovePassword();
        }
    }
}