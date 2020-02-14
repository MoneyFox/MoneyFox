using MoneyFox.Uwp.ViewModels.Settings;

namespace MoneyFox.Uwp.ViewModels.DesignTime
{
    public class DesignTimeSettingsViewModel : ISettingsViewModel
    {
        public ISettingsBackgroundJobViewModel BackgroundJobViewModel { get; }
        public ISettingsPersonalizationViewModel PersonalizationViewModel { get; }

        public IRegionalSettingsViewModel RegionalSettingsViewModel { get; }
    }
}
