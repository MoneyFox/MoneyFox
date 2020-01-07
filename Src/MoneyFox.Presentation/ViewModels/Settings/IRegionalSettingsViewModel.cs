using MoneyFox.Application.Common.CurrencyConversion.Models;
using MoneyFox.Presentation.Commands;
using System.Collections.ObjectModel;

namespace MoneyFox.Presentation.ViewModels.Settings
{
    public interface IRegionalSettingsViewModel
    {
        ObservableCollection<Currency> AvailableCurrencies { get; }

        AsyncCommand LoadAvailableCurrenciesCommand { get; }
    }
}
