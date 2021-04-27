using MoneyFox.Application.Common.CurrencyConversion.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace MoneyFox.Application.Common.CurrencyConversion
{
    public static class RequestHelper
    {
#pragma warning disable S1075 // URIs should not be hardcoded
        private const string BASE_URL = "https://free.currconv.com/api/v7/";
#pragma warning restore S1075 // URIs should not be hardcoded

        public static List<Currency> GetAllCurrencies(string apiKey)
        {
            var url = new Uri($"{BASE_URL}currencies?apiKey={apiKey}");

            string jsonString = GetResponse(url);

            JToken[] data = JObject.Parse(jsonString)["results"].ToArray();
            return data.Select(item => item.First.ToObject<Currency>()).ToList();
        }

        public static double ExchangeRate(string from, string to, string apiKey)
        {
            var url = new Uri($"{BASE_URL}convert?q={from}_{to}&compact=ultra&apiKey={apiKey}");

            string jsonString = GetResponse(url);
            return JObject.Parse(jsonString).First.First["val"].ToObject<double>();
        }

        private static string GetResponse(Uri url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using var response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            using var reader = new StreamReader(stream);

            string jsonString = reader.ReadToEnd();


            return jsonString;
        }
    }
}
