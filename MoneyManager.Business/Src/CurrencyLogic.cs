using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MoneyManager.Business.Src
{
    public class CurrencyLogic
    {
        private const string CURRENCY_SERVICE_URL = "http://www.freecurrencyconverterapi.com/api/convert?q={0}&compact=y";
        private const string COUNTRIES_SERVICE_URL = "http://www.freecurrencyconverterapi.com/api/v2/countries";

        private static HttpClient httpClient = new HttpClient();

        public CurrencyLogic()
        {
            httpClient.DefaultRequestHeaders.Add("user-agent",
                "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
        }

        public async static Task GetSupportedCurrencies()
        {
            var jsonString = await GetJsonFromService(COUNTRIES_SERVICE_URL);
            ParseCountries(jsonString);
        }

        private static void ParseCountries(string jsonString)
        {
            var countries = JsonConvert.DeserializeAnonymousType(jsonString, typeof(CurrencyServiceDTO));
        }

        public async static Task<double> GetCurrencyRatio(string currencyFrom, string currencyTo)
        {
            string currencyFromTo = string.Format("{0}-{1}", currencyFrom.ToUpper(), currencyTo.ToUpper());
            string url = string.Format(CURRENCY_SERVICE_URL, currencyFromTo);

            string jsonString = await GetJsonFromService(url); // "{\"CHF-EUR\":{\"val\":1.2212}}";
            jsonString = jsonString.Replace(currencyFromTo, "Conversion");

            return ParseToExchangeRate(jsonString);
        }

        private static double ParseToExchangeRate(string jsonString)
        {
            var typeExample =
                new
                {
                    Conversion = new
                                 {
                                     val = ""
                                 }
                };

            var currency = JsonConvert.DeserializeAnonymousType(jsonString, typeExample);
            return Double.Parse(currency.Conversion.val, CultureInfo.CurrentCulture);
        }

        private static async Task<string> GetJsonFromService(string url)
        {
            PrepareHttpClient();
            var req = new HttpRequestMessage(HttpMethod.Get, url);
            HttpResponseMessage response = await httpClient.SendAsync(req);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        private static void PrepareHttpClient()
        {
            httpClient = new HttpClient { BaseAddress = new Uri("https://api.SmallInvoice.com/") };
            httpClient.DefaultRequestHeaders.Add("user-agent",
                "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
        }
    }
}
