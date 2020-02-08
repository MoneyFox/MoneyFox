using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Presentation.Models;

namespace MoneyFox.Presentation.ViewModels.Settings
{
    public interface ISettingsViewModel
    {
        /// <summary>
        ///     Contains all available Settings items.
        /// </summary>
        ObservableCollection<SettingsSelectorType> SettingsList { get; }

        /// <summary>
        ///     Navigate to a concrete settings page.
        ///     Used in Xamarin Forms.
        /// </summary>
        RelayCommand<SettingsSelectorType> GoToSettingCommand { get; }

        /// <summary>
        ///     View Model for the Background job part.
        /// </summary>
        ISettingsBackgroundJobViewModel BackgroundJobViewModel { get; }

        IRegionalSettingsViewModel RegionalSettingsViewModel { get; }

        ISettingsPersonalizationViewModel PersonalizationViewModel { get; }
    }
}