using GalaSoft.MvvmLight;
using MoneyFox.Application.Common.Interfaces;

namespace MoneyFox.Presentation.ViewModels.Settings
{
    /// <summary>
    ///     ViewModel for the settings view.
    /// </summary>
    public class SettingsViewModel : ViewModelBase, ISettingsViewModel
    {
        private readonly INavigationService navigationService;

        public SettingsViewModel(INavigationService navigationService,
                                 IAboutViewModel aboutViewModel,
                                 ISettingsBackgroundJobViewModel settingsBackgroundJobViewModel,
                                 IRegionalSettingsViewModel regionalSettingsViewModel,
                                 ISettingsPersonalizationViewModel settingsPersonalizationViewModel)
        {
            this.navigationService = navigationService;

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
