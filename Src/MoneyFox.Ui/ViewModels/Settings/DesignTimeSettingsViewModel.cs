namespace MoneyFox.Ui.ViewModels.Settings;

using System.Collections.ObjectModel;
using System.Globalization;

public class DesignTimeSettingsViewModel : ISettingsViewModel
{
    public ObservableCollection<CultureInfo> AvailableCultures => new();

    public CultureInfo SelectedCulture { get; set; } = CultureInfo.CurrentCulture;

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }
}
