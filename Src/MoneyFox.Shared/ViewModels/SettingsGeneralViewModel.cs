using MoneyFox.Shared.Helpers;
using MvvmCross.Localization;

namespace MoneyFox.Shared.ViewModels
{
    public class SettingsGeneralViewModel : BaseViewModel
    {
        /// <summary>
        ///     Provides an TextSource for the translation binding on this page.
        /// </summary>
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("", GetType().Name);

        public bool IsAutoBackupEnabled
        {
            get { return SettingsHelper.IsBackupAutouploadEnabled; }
            set
            {
                SettingsHelper.IsBackupAutouploadEnabled = value;
                RaisePropertyChanged();
            }
        }
    }
}