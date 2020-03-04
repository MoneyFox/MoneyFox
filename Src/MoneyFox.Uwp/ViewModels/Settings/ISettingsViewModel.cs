namespace MoneyFox.Uwp.ViewModels.Settings
{
    public interface ISettingsViewModel
    {
        /// <summary>
        /// View Model for the Background job part.
        /// </summary>
        ISettingsBackgroundJobViewModel BackgroundJobViewModel { get; }

        IRegionalSettingsViewModel RegionalSettingsViewModel { get; }

        ISettingsPersonalizationViewModel PersonalizationViewModel { get; }
    }
}
