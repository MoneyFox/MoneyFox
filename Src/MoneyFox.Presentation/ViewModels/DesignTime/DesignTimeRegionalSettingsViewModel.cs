using System;
using System.Collections.ObjectModel;
using System.Globalization;
using MoneyFox.Presentation.Commands;
using MoneyFox.Presentation.ViewModels.Settings;

namespace MoneyFox.Presentation.ViewModels.DesignTime
{
    public class DesignTimeRegionalSettingsViewModel : IRegionalSettingsViewModel
    {
        public ObservableCollection<CultureInfo> AvailableCultures => new ObservableCollection<CultureInfo>
        {
            new CultureInfo("de-CH"),
            new CultureInfo("en-US")
        };

        public AsyncCommand LoadAvailableCulturesCommand => throw new NotImplementedException();

        public CultureInfo SelectedCulture { get; set; } = CultureInfo.CurrentCulture;
    }
}
