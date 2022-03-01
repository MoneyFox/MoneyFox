namespace MoneyFox.Win.ViewModels.Settings;

using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;

public class DesignTimeSettingsViewModel : ISettingsViewModel
{
    public ObservableCollection<CultureInfo> AvailableCultures => new();

    public CultureInfo SelectedCulture { get; set; } = CultureInfo.CurrentCulture;

    public Task InitializeAsync() => Task.CompletedTask;
}