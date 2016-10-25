using MoneyFox.Foundation.Interfaces;

namespace MoneyFox.Business.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel(ISettingsManager settingsManager,
            IPasswordStorage passwordStorage,
            ITileManager tileManager,
            IDialogService dialogService)
        {
            SettingsGeneralViewModel = new SettingsGeneralViewModel(settingsManager);
            SettingsSecurityViewModel = new SettingsSecurityViewModel(settingsManager, passwordStorage, dialogService);
            SettingsShortcutsViewModel = new SettingsShortcutsViewModel(settingsManager, tileManager);
            SettingsPersonalizationViewModel = new SettingsPersonalizationViewModel(settingsManager);
        }

        public SettingsGeneralViewModel SettingsGeneralViewModel { get; }
        public SettingsSecurityViewModel SettingsSecurityViewModel { get; }
        public SettingsShortcutsViewModel SettingsShortcutsViewModel { get; }
        public SettingsPersonalizationViewModel SettingsPersonalizationViewModel { get; }
    }
}