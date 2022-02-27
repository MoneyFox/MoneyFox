namespace MoneyFox.Win.ViewModels.Settings;

using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;

public interface ISettingsViewModel
{
    ObservableCollection<CultureInfo> AvailableCultures { get; }

    CultureInfo SelectedCulture { get; set; }

    Task InitializeAsync();
}