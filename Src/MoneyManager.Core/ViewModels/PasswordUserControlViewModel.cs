using Cirrious.MvvmCross.ViewModels;
using MoneyManager.DataAccess;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class PasswordUserControlViewModel : BaseViewModel
    {
        private readonly SettingDataAccess settings;

        public PasswordUserControlViewModel(SettingDataAccess settings)
        {
            this.settings = settings;
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

        private void SavePassword()
        {
            throw new System.NotImplementedException();
        }
    }
}
