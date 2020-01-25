using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyFox.Application.Common.CurrencyConversion.Models;

namespace MoneyFox.Application.Common.CurrencyConversion
{
    public interface ICurrencyConverterService
    {
        double Convert(double amount, string from, string to);

        Task<double> ConvertAsync(double amount, string from, string to);

        List<Currency> GetAllCurrencies();

        Task<List<Currency>> GetAllCurrenciesAsync();
    }
}
