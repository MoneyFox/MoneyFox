using System.Threading.Tasks;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Models;
using MoneyFox.Foundation.Resources;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Plugin.Connectivity.Abstractions;

namespace MoneyFox.Business.ViewModels
{
    public interface ISettingsViewModel : IBaseViewModel
    {
        /// <summary>
        ///     Contains all available Settingsitems.
        /// </summary>
        MvxObservableCollection<SettingsSelectorType> SettingsList { get; }

    }

    /// <summary>
    ///     ViewModel for the settings view.
    /// </summary>
    public class SettingsViewModel : BaseViewModel, ISettingsViewModel
    {

        public SettingsViewModel(ISettingsManager settingsManager,
                                 IPasswordStorage passwordStorage,
                                 ITileManager tileManager,
                                 IBackgroundTaskManager backgroundTaskManager,
                                 IDialogService dialogService,
                                 IBackupManager backupManager,
                                 IConnectivity connectivity)
        {
            SettingsGeneralViewModel = new SettingsGeneralViewModel(backupManager, dialogService, connectivity, settingsManager, backgroundTaskManager);
            SettingsSecurityViewModel = new SettingsSecurityViewModel(settingsManager, passwordStorage, dialogService);
            SettingsShortcutsViewModel = new SettingsShortcutsViewModel(settingsManager, tileManager);
            SettingsPersonalizationViewModel = new SettingsPersonalizationViewModel(settingsManager);
        }

        public SettingsGeneralViewModel SettingsGeneralViewModel { get; }

        public SettingsSecurityViewModel SettingsSecurityViewModel { get; }

        public SettingsShortcutsViewModel SettingsShortcutsViewModel { get; }

        public SettingsPersonalizationViewModel SettingsPersonalizationViewModel { get; }

        public string Title => Strings.SettingsLabel;

        /// <inheritdoc />
        public MvxObservableCollection<SettingsSelectorType> SettingsList => new MvxObservableCollection<SettingsSelectorType>
        {
            new SettingsSelectorType
            {
                Name = Strings.CategoriesLabel,
                Description = Strings.CategoriesSettingsDescription,
                Type = SettingsType.Categories
            },
            new SettingsSelectorType
            {
                Name = Strings.BackupLabel,
                Description = Strings.BackupSettingsDescription,
                Type = SettingsType.Backup
            },
            new SettingsSelectorType
            {
                Name = Strings.AboutLabel,
                Description = Strings.AboutSettingsDescription,
                Type = SettingsType.About
            }
        };
    }
}