using MoneyFox.Presentation.ViewModels.Settings;

namespace MoneyFox.Presentation.ViewModels.DesignTime
{
    public class DesignTimeSettingsViewModel : ISettingsViewModel
    {
        public ISettingsBackgroundJobViewModel BackgroundJobViewModel { get; }
        public ISettingsPersonalizationViewModel PersonalizationViewModel { get; }

        public IRegionalSettingsViewModel RegionalSettingsViewModel { get; }
    }
}
