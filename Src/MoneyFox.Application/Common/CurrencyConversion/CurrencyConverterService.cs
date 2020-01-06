using MoneyFox.Application.Common.CurrencyConversion.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyFox.Application.Common.CurrencyConversion
{
    public class CurrencyConverterService : ICurrencyConverterService
    {
        private string apiKey;

        public CurrencyConverterService(string apiKey)
        {
            this.apiKey = apiKey;
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
            return RequestHelper.GetAllCurrencies(apiKey);
        }
        
        public async Task<List<Currency>> GetAllCurrenciesAsync()
        {
            return await Task.Run(() => GetAllCurrencies());
        }
    }
}
