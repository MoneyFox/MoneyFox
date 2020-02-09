using GalaSoft.MvvmLight;

namespace MoneyFox.Uwp.ViewModels.Settings
{
    /// <summary>
    ///     ViewModel for the settings view.
    /// </summary>
    public class SettingsViewModel : ViewModelBase, ISettingsViewModel
    {
        public SettingsViewModel(IAboutViewModel aboutViewModel,
                                 ISettingsBackgroundJobViewModel settingsBackgroundJobViewModel,
                                 IRegionalSettingsViewModel regionalSettingsViewModel,
                                 ISettingsPersonalizationViewModel settingsPersonalizationViewModel)
        {
            AboutViewModel = aboutViewModel;
            BackgroundJobViewModel = settingsBackgroundJobViewModel;
            RegionalSettingsViewModel = regionalSettingsViewModel;
            PersonalizationViewModel = settingsPersonalizationViewModel;
        }

        public IAboutViewModel AboutViewModel { get; }

        public ISettingsBackgroundJobViewModel BackgroundJobViewModel { get; private set; }

        public ISettingsPersonalizationViewModel PersonalizationViewModel { get; private set; }

        public IRegionalSettingsViewModel RegionalSettingsViewModel { get; private set; }
    }
}
