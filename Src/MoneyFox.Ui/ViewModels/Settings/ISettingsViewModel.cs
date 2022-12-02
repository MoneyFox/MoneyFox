namespace MoneyFox.Ui.ViewModels.Settings;

using System.Collections.ObjectModel;
using System.Globalization;

public interface ISettingsViewModel
{
    ObservableCollection<CultureInfo> AvailableCultures { get; }

    CultureInfo SelectedCulture { get; set; }

    Task InitializeAsync();
}
