using MoneyFox.Uwp.ViewModels.Settings;

namespace MoneyFox.Uwp.ViewModels.DesignTime
{
    public class DesignTimeSettingsViewModel : ISettingsViewModel
    {
        public ISettingsBackgroundJobViewModel BackgroundJobViewModel { get; } = null!;

        public ISettingsPersonalizationViewModel PersonalizationViewModel { get; } = null!;

        public IRegionalSettingsViewModel RegionalSettingsViewModel { get; } = null!;
    }
}
