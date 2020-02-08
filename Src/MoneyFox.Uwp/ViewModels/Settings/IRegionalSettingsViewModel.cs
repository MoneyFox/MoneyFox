using System.Collections.ObjectModel;
using System.Globalization;
using MoneyFox.Ui.Shared.Commands;

namespace MoneyFox.Uwp.ViewModels.Settings
{
    public interface IRegionalSettingsViewModel
    {
        CultureInfo SelectedCulture { get; set; }

        ObservableCollection<CultureInfo> AvailableCultures { get; }

        AsyncCommand LoadAvailableCulturesCommand { get; }
    }
}
