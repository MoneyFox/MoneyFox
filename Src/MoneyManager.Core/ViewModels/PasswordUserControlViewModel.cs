using System;
using Beezy.MvvmCross.Plugins.SecureStorage;
using Cirrious.MvvmCross.ViewModels;
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
        private readonly IMvxProtectedData protectedData;
        private readonly SettingDataAccess settings;

        public PasswordUserControlViewModel(SettingDataAccess settings,
            IMvxProtectedData protectedData, IDialogService dialogService)
        {
            this.settings = settings;
            this.protectedData = protectedData;
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

            IsPasswortActive = true;
            protectedData.Protect(PASSWORD_KEY, Password);

            dialogService.ShowMessage(Strings.PasswordSavedTitle, Strings.PasswordSavedMessage);
        }

        private void LoadData()
        {
            if (IsPasswortActive)
            {
                Password = protectedData.Unprotect(PASSWORD_KEY);
                PasswordConfirmation = protectedData.Unprotect(PASSWORD_KEY);
            }
        }

        private void RemovePassword()
        {
            protectedData.Remove(PASSWORD_KEY);
        }
    }
}