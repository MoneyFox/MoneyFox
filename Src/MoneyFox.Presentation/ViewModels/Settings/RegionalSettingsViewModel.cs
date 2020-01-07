using MoneyFox.Application.Common.CurrencyConversion;
using MoneyFox.Application.Common.CurrencyConversion.Models;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Presentation.Commands;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MoneyFox.Presentation.ViewModels.Settings
{
    public class RegionalSettingsViewModel : IRegionalSettingsViewModel
    {
        private readonly ICurrencyConverterService currencyConverterService;
        private readonly IDialogService dialogService;

        public RegionalSettingsViewModel(ICurrencyConverterService currencyConverterService, IDialogService dialogService)
        {
            this.currencyConverterService = currencyConverterService;
            this.dialogService = dialogService;

            AvailableCurrencies = new ObservableCollection<Currency>();
        }

        public ObservableCollection<Currency> AvailableCurrencies { get; }

        public AsyncCommand LoadAvailableCurrenciesCommand => new AsyncCommand(LoadAvailableCurrenciesAsync);

        private async Task LoadAvailableCurrenciesAsync()
        {
            await dialogService.ShowLoadingDialogAsync();

            var currencies = await currencyConverterService.GetAllCurrenciesAsync();
            currencies.ForEach(AvailableCurrencies.Add);

            await dialogService.HideLoadingDialogAsync();
        }
    }
}
