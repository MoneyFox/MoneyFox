using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MoneyManager.DataAccess.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MoneyManager.Business.Logic
{
    public class CurrencyLogic
    {
        private const string CURRENCY_SERVICE_URL =
            "http://www.freecurrencyconverterapi.com/api/convert?q={0}&compact=y";

        private const string COUNTRIES_SERVICE_URL = "http://www.freecurrencyconverterapi.com/api/v2/countries";

        private static HttpClient httpClient = new HttpClient();

        public CurrencyLogic()
        {
            httpClient.DefaultRequestHeaders.Add("user-agent",
                "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
        }

        public static async Task<List<Country>> GetSupportedCountries()
        {
            var jsonString = await GetJsonFromService(COUNTRIES_SERVICE_URL);

            var json = JsonConvert.DeserializeObject(jsonString) as JContainer;

            return (from JProperty token in json.Children().Children().Children()
                select new Country
                {
                    Abbreviation = token.Name,
                    CurrencyID = token.Value["currencyId"].ToString(),
                    CurrencyName = token.Value["currencyName"].ToString(),
                    Name = token.Value["name"].ToString(),
                    Alpha3 = token.Value["alpha3"].ToString(),
                    ID = token.Value["id"].ToString(),
                }).ToList();
        }

        public static async Task<double> GetCurrencyRatio(string currencyFrom, string currencyTo)
        {
            var currencyFromTo = string.Format("{0}-{1}", currencyFrom.ToUpper(), currencyTo.ToUpper());
            var url = string.Format(CURRENCY_SERVICE_URL, currencyFromTo);

            var jsonString = await GetJsonFromService(url); // "{\"CHF-EUR\":{\"val\":1.2212}}";
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
            httpClient = new HttpClient {BaseAddress = new Uri("https://api.SmallInvoice.com/")};
            httpClient.DefaultRequestHeaders.Add("user-agent",
                "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
        }
    }
}