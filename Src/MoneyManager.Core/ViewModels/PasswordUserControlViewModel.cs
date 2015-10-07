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
        public bool IsPasswordSet
        {
            get { return settings.PasswordRequired; }
            set { settings.PasswordRequired = value; }
        }
    }
}
