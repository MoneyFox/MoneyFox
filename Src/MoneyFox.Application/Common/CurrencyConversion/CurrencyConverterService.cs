using MoneyFox.Application.Common.CurrencyConversion.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyFox.Application.Common.CurrencyConversion
{
    public class CurrencyConverterService : ICurrencyConverterService
    {
        private readonly string apiKey;

        public CurrencyConverterService(TokenObject tokenObject)
        {
            apiKey = tokenObject.CurrencyConverterApi;
        }

        public double Convert(double amount, string from, string to)
        {
            return RequestHelper.ExchangeRate(from, to, apiKey) * amount;
        }

        public async Task<double> ConvertAsync(double amount, string from, string to)
        {
            return await Task.Run(() => Convert(amount, from, to));
        }

        public List<Currency> GetAllCurrencies()
        {
            return RequestHelper.GetAllCurrencies(apiKey).OrderBy(x => x.CurrencyName).ToList();
        }

        public async Task<List<Currency>> GetAllCurrenciesAsync()
        {
            return await Task.Run(() => GetAllCurrencies());
        }
    }
}
