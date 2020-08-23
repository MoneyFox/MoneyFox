namespace MoneyFox.Uwp.ViewModels.Settings
{
    public interface ISettingsViewModel
    {
        IRegionalSettingsViewModel RegionalSettingsViewModel { get; }

        ISettingsPersonalizationViewModel PersonalizationViewModel { get; }
    }
}
