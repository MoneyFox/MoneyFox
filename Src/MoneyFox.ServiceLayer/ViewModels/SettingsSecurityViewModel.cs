using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Interfaces;
using MvvmCross.Commands;
using MvvmCross.Localization;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public class SettingsSecurityViewModel : BaseNavigationViewModel, ISettingsSecurityViewModel
    {
        private readonly ISettingsFacade settingsFacade;
        private readonly IDialogService dialogService;
        private readonly IPasswordStorage passwordStorage;
        private string password;
        private string passwordConfirmation;

        public SettingsSecurityViewModel(ISettingsFacade settingsFacade, 
                                         IPasswordStorage passwordStorage, 
                                         IDialogService dialogService,
                                         IMvxLogProvider logProvider,
                                         IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            this.settingsFacade = settingsFacade;
            this.passwordStorage = passwordStorage;
            this.dialogService = dialogService;
        }

        /// <summary>
        ///     Grants the GUI access to the password setting.
        /// </summary>
        public bool IsPasswortActive
        {
            get => settingsFacade.PasswordRequired;
            set
            {
                settingsFacade.PasswordRequired = value;
                RaisePropertyChanged();
            }
        }


        public bool IsPassportActive
        {
            get => settingsFacade.PassportEnabled;
            set
            {

                settingsFacade.PassportEnabled = value;
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