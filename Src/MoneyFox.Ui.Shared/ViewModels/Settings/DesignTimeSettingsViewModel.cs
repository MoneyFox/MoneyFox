using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;

namespace MoneyFox.Ui.Shared.ViewModels.Settings
{
    public class DesignTimeSettingsViewModel : ISettingsViewModel
    {
        public ObservableCollection<CultureInfo> AvailableCultures => new ObservableCollection<CultureInfo>();

        public CultureInfo SelectedCulture { get; set; } = CultureInfo.CurrentCulture;

        public Task InitializeAsync() { return Task.CompletedTask; }
    }
}
