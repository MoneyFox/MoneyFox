using MoneyFox.Ui.Shared.Commands;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MoneyFox.Presentation.ViewModels.Settings
{
    public interface IRegionalSettingsViewModel
    {
        CultureInfo SelectedCulture { get; set; }

        ObservableCollection<CultureInfo> AvailableCultures { get; }

        AsyncCommand LoadAvailableCulturesCommand { get; }
    }
}
