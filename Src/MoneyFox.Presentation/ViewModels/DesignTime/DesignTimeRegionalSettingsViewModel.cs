using MoneyFox.Application.Common.CurrencyConversion.Models;
using MoneyFox.Presentation.Commands;
using MoneyFox.Presentation.ViewModels.Settings;
using System.Collections.ObjectModel;

namespace MoneyFox.Presentation.ViewModels.DesignTime
{
    public class DesignTimeRegionalSettingsViewModelFoo : IRegionalSettingsViewModel
    {
        public ObservableCollection<Currency> AvailableCurrencies => new ObservableCollection<Currency>
        {
            new Currency
            {
                CurrencyName = "Schweizer Franken",
                CurrencySymbol = "CHF",
                Id = "CHF"
            },
            new Currency
            {
                CurrencyName = "US Dollar",
                CurrencySymbol = "$",
                Id = "USD"
            }
        };

        public AsyncCommand LoadAvailableCurrenciesCommand => throw new System.NotImplementedException();
    }
}
