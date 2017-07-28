using MoneyFox.Foundation.Interfaces;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels
{
    public class SettingsViewModel : MvxViewModel
    {
        public SettingsViewModel(ISettingsManager settingsManager,
            IPasswordStorage passwordStorage,
            ITileManager tileManager,
            IBackgroundTaskManager backgroundTaskManager,
            IDialogService dialogService,
            IThemeService themeService)
        {
            SettingsGeneralViewModel = new SettingsGeneralViewModel(settingsManager, backgroundTaskManager);
            SettingsSecurityViewModel = new SettingsSecurityViewModel(settingsManager, passwordStorage, dialogService);
            SettingsShortcutsViewModel = new SettingsShortcutsViewModel(settingsManager, tileManager);
            SettingsPersonalizationViewModel = new SettingsPersonalizationViewModel(themeService);
        }

        public SettingsGeneralViewModel SettingsGeneralViewModel { get; }
        public SettingsSecurityViewModel SettingsSecurityViewModel { get; }
        public SettingsShortcutsViewModel SettingsShortcutsViewModel { get; }
        public SettingsPersonalizationViewModel SettingsPersonalizationViewModel { get; }
    }
}