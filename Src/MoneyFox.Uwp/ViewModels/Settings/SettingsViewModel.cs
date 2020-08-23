using GalaSoft.MvvmLight;

namespace MoneyFox.Uwp.ViewModels.Settings
{
    /// <summary>
    /// ViewModel for the settings view.
    /// </summary>
    public class SettingsViewModel : ViewModelBase, ISettingsViewModel
    {
        public SettingsViewModel(IRegionalSettingsViewModel regionalSettingsViewModel,
                                 ISettingsPersonalizationViewModel settingsPersonalizationViewModel)
        {
            RegionalSettingsViewModel = regionalSettingsViewModel;
            PersonalizationViewModel = settingsPersonalizationViewModel;
        }

        public ISettingsPersonalizationViewModel PersonalizationViewModel { get; private set; }

        public IRegionalSettingsViewModel RegionalSettingsViewModel { get; private set; }
    }
}
