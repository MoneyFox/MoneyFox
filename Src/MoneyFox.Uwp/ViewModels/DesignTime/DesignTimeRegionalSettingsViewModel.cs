using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Uwp.ViewModels.Settings;
using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MoneyFox.Uwp.ViewModels.DesignTime
{
    public class DesignTimeRegionalSettingsViewModel : IRegionalSettingsViewModel
    {
        public ObservableCollection<CultureInfo> AvailableCultures
                                                 => new ObservableCollection<CultureInfo>
        {
            new CultureInfo("de-CH"),
            new CultureInfo("en-US")
        };

        public AsyncCommand LoadAvailableCulturesCommand => throw new NotImplementedException();

        public CultureInfo SelectedCulture { get; set; } = CultureInfo.CurrentCulture;
    }
}
