using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MvvmCross.Commands;
using MvvmCross.Localization;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace MoneyFox.Business.ViewModels
{
    public class SettingsSecurityViewModel : BaseViewModel, ISettingsSecurityViewModel
    {
        private readonly ISettingsManager settingsManager;
        private readonly IDialogService dialogService;
        private readonly IPasswordStorage passwordStorage;
        private string password;
        private string passwordConfirmation;

        public SettingsSecurityViewModel(ISettingsManager settingsManager, 
                                         IPasswordStorage passwordStorage, 
                                         IDialogService dialogService,
                                         IMvxLogProvider logProvider,
                                         IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            this.settingsManager = settingsManager;
            this.passwordStorage = passwordStorage;
            this.dialogService = dialogService;
        }

        /// <summary>
        ///     Grants the GUI access to the password setting.
        /// </summary>
        public bool IsPasswortActive
        {
            get => settingsManager.PasswordRequired;
            set
            {
                settingsManager.PasswordRequired = value;
                RaisePropertyChanged();
            }
        }


        public bool IsPassportActive
        {
            get => settingsManager.PassportEnabled;
            set
            {
                
                settingsManager.PassportEnabled = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Provides an TextSource for the translation binding on this page.
        /// </summary>
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("", GetType().Name);

        /// <summary>
        ///     The password that the user set.
        /// </summary>
        public string Password
        {
            get => password;
            set
            {
                password = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     The password confirmation the user entered.
        /// </summary>
        public string PasswordConfirmation
        {
            get => passwordConfirmation;
            set
            {
                passwordConfirmation = value;
                RaisePropertyChanged();
            }
        }

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
        public MvxCommand UnloadCommand => new MvxCommand(RemovePassword);

        private void SavePassword()
        {
            if (string.IsNullOrEmpty(Password))
            {
                dialogService.ShowMessage(Strings.PasswordEmptyTitle,
                                          Strings.PasswordEmptyMessage);
                return;
            }

            if (Password != PasswordConfirmation)
            {
                dialogService.ShowMessage(Strings.PasswordConfirmationWrongTitle,
                    Strings.PasswordConfirmationWrongMessage);
                return;
            }

            passwordStorage.SavePassword(Password);

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
            if (!IsPasswortActive)
            {
                passwordStorage.RemovePassword();
            }

            //  Deactivate option again if no password was entered
            if (IsPasswortActive && string.IsNullOrEmpty(Password))
            {
                IsPasswortActive = false;
            }
        }
    }
}