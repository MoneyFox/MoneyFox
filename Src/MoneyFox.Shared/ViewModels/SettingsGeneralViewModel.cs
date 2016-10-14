using MoneyFox.Shared.Interfaces;
using MvvmCross.Localization;

namespace MoneyFox.Shared.ViewModels
{
    public class SettingsGeneralViewModel : BaseViewModel
    {
        private readonly ISettingsManager settingsManager;

        public SettingsGeneralViewModel(ISettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
        }

        /// <summary>
        ///     Provides an TextSource for the translation binding on this page.
        /// </summary>
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("", GetType().Name);

        public bool IsAutoBackupEnabled
        {
            get { return settingsManager.IsBackupAutouploadEnabled; }
            set
            {
                settingsManager.IsBackupAutouploadEnabled = value;
                RaisePropertyChanged();
            }
        }
    }
}