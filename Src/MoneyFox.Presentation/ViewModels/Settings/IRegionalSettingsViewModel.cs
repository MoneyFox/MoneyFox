using System.Collections.ObjectModel;
using System.Globalization;
using MoneyFox.Presentation.Commands;

namespace MoneyFox.Presentation.ViewModels.Settings
{
    public interface IRegionalSettingsViewModel
    {
        CultureInfo SelectedCulture { get; set; }

        ObservableCollection<CultureInfo> AvailableCultures { get; }

        AsyncCommand LoadAvailableCulturesCommand { get; }
    }
}
