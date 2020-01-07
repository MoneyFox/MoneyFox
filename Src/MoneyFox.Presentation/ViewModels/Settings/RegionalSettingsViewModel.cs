using MoneyFox.Application.Common.CurrencyConversion;
using MoneyFox.Application.Common.CurrencyConversion.Models;
using MoneyFox.Presentation.Commands;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MoneyFox.Presentation.ViewModels.Settings
{
    public class RegionalSettingsViewModel : IRegionalSettingsViewModel
    {
        private readonly ICurrencyConverterService currencyConverterService;

        public RegionalSettingsViewModel(ICurrencyConverterService currencyConverterService)
        {
            this.currencyConverterService = currencyConverterService;

            AvailableCurrencies = new ObservableCollection<Currency>();
        }

        public ObservableCollection<Currency> AvailableCurrencies { get; }

        public AsyncCommand LoadAvailableCurrenciesCommand => new AsyncCommand(LoadAvailableCurrencies);
        
        private async Task LoadAvailableCurrencies()
        {
            var currencies = await currencyConverterService.GetAllCurrenciesAsync();

            currencies.ForEach(AvailableCurrencies.Add);
        }
    }
}
