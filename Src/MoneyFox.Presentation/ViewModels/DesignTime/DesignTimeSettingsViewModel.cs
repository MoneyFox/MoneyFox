using System.Collections.ObjectModel;
using System.Globalization;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Application.Resources;
using MoneyFox.Domain;
using MoneyFox.Presentation.Models;
using MoneyFox.ServiceLayer.Utilities;

namespace MoneyFox.Presentation.ViewModels.DesignTime
{
    public class DesignTimeSettingsViewModel : ISettingsViewModel
    {
        public DesignTimeSettingsViewModel()
        {
            Resources = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);
        }

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
            },
        };

        /// <inheritdoc />
        public RelayCommand<SettingsSelectorType> GoToSettingCommand { get; }

        public ISettingsBackgroundJobViewModel BackgroundJobViewModel { get; }
        public ISettingsPersonalizationViewModel PersonalizationViewModel { get; }

        public LocalizedResources Resources { get; }
    }
}
