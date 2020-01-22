using MoneyFox.Application.Common.CurrencyConversion.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace MoneyFox.Application.Common.CurrencyConversion
{
    public static class RequestHelper
    {
        public const string BASE_URL = "https://free.currconv.com/api/v7/";

        public static List<Currency> GetAllCurrencies(string apiKey)
        {
            string url = BASE_URL + $"currencies?apiKey={apiKey}";

            var jsonString = GetResponse(url);

            var data = JObject.Parse(jsonString)["results"].ToArray();
            return data.Select(item => item.First.ToObject<Currency>()).ToList();
        }

        public static double ExchangeRate(string from, string to, string apiKey)
        {
            string url = BASE_URL + $"convert?q={from}_{to}&compact=ultra&apiKey={apiKey}";

            var jsonString = GetResponse(url);
            return JObject.Parse(jsonString).First.First["val"].ToObject<double>();
        }

        private static string GetResponse(string url)
        {
            string jsonString;

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (var response = (HttpWebResponse)request.GetResponse())
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                jsonString = reader.ReadToEnd();
            }

            return jsonString;
        }
    }
}
