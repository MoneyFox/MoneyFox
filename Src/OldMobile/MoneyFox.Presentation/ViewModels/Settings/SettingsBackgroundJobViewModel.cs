using GalaSoft.MvvmLight;
using MoneyFox.Application.Common.Facades;

namespace MoneyFox.Presentation.ViewModels.Settings
{
    public interface ISettingsBackgroundJobViewModel
    {
        /// <summary>
        /// Indicates if the autobackup is enabled or disabled.
        /// </summary>
        bool IsAutoBackupEnabled { get; }
    }

    /// <inheritdoc cref="ISettingsBackgroundJobViewModel"/>
    public class SettingsBackgroundJobViewModel : ViewModelBase, ISettingsBackgroundJobViewModel
    {
        private readonly ISettingsFacade settingsFacade;

        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsBackgroundJobViewModel(ISettingsFacade settingsFacade)
        {
            this.settingsFacade = settingsFacade;
        }

        /// <inheritdoc/>
        public bool IsAutoBackupEnabled
        {
            get => settingsFacade.IsBackupAutouploadEnabled;
            set
            {
                if(settingsFacade.IsBackupAutouploadEnabled == value)
                    return;

                settingsFacade.IsBackupAutouploadEnabled = value;
                RaisePropertyChanged();
            }
        }
    }
}
