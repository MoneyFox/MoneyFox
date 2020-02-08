using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Application.Resources;
using MoneyFox.Domain;
using MoneyFox.Presentation.Models;
using MoneyFox.Presentation.ViewModels.Settings;

namespace MoneyFox.Presentation.ViewModels.DesignTime
{
    public class DesignTimeSettingsViewModel : ISettingsViewModel
    {
        /// <inheritdoc />
        public ObservableCollection<SettingsSelectorType> SettingsList => new ObservableCollection<SettingsSelectorType>
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
            }
        };

        /// <inheritdoc />
        public RelayCommand<SettingsSelectorType> GoToSettingCommand { get; }

        public ISettingsBackgroundJobViewModel BackgroundJobViewModel { get; }
        public ISettingsPersonalizationViewModel PersonalizationViewModel { get; }

        public IRegionalSettingsViewModel RegionalSettingsViewModel { get; }
    }
}
