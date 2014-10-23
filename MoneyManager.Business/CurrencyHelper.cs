using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MoneyManager.Business
{
    internal class CurrencyHelper
    {
        private const string currencyServiceUrl = "http://www.freecurrencyconverterapi.com/api/convert?q={0}&compact=y";
        private static readonly HttpClient httpClient = new HttpClient();

        public CurrencyHelper()
        {
            httpClient.DefaultRequestHeaders.Add("user-agent",
                "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
        }

        public static void GetSupportedCurrencies()
        {
        }

        public static double GetCurrencyRatio(string currencyFrom, string currencyTo)
        {
            var httpClient = new HttpClient {BaseAddress = new Uri("https://api.SmallInvoice.com/")};
            httpClient.DefaultRequestHeaders.Add("user-agent",
                "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");

            string currencyFromTo = string.Format("{0}-{1}", currencyFrom.ToUpper(), currencyTo.ToUpper());
            string currenciesFromTo = string.Format(currencyServiceUrl, currencyFromTo);

            //Get Value from Service
            //

            string jsonString = "{\"EUR-CHF\":{\"val\":1.2212}}";

            jsonString = jsonString.Replace(currenciesFromTo, "Conversion");

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
            var req = new HttpRequestMessage(HttpMethod.Get, url);
            HttpResponseMessage response = await httpClient.SendAsync(req);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            return content;
        }
    }
}