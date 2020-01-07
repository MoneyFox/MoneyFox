using MoneyFox.Presentation.Commands;
using MoneyFox.Presentation.ViewModels.Settings;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MoneyFox.Presentation.ViewModels.DesignTime
{
    public class DesignTimeRegionalSettingsViewModel : IRegionalSettingsViewModel
    {
        public ObservableCollection<CultureInfo> AvailableCultures => new ObservableCollection<CultureInfo>
        {
            new CultureInfo("de-CH"),
            new CultureInfo("en-US")
        };

        public AsyncCommand LoadAvailableCulturesCommand => throw new System.NotImplementedException();

        public CultureInfo SelectedCulture { get; set; } = CultureInfo.CurrentCulture;
        public int SelectedCultureIndex { get => 1; set => throw new System.NotImplementedException(); }
    }
}
