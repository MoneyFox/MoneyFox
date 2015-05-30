using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MoneyManager.Business.Manager {
    public class CurrencyManager {
        private const string CURRENCY_SERVICE_URL = "http://www.freecurrencyconverterapi.com/api/convert?q={0}&compact=y";
        private const string COUNTRIES_SERVICE_URL = "http://www.freecurrencyconverterapi.com/api/v2/countries";

        private readonly IJsonService _jsonService;

        public CurrencyManager(IJsonService jsonService) {
            _jsonService = jsonService;
        }

        public async Task<List<Country>> GetSupportedCountries() {
            try {
                string jsonString = await _jsonService.GetJsonFromService(COUNTRIES_SERVICE_URL);

                var json = JsonConvert.DeserializeObject(jsonString) as JContainer;

                return (from JProperty token in json.Children().Children().Children()
                    select new Country {
                        Abbreviation = token.Name,
                        CurrencyID = token.Value["currencyId"].ToString(),
                        CurrencyName = token.Value["currencyName"].ToString(),
                        Name = token.Value["name"].ToString(),
                        Alpha3 = token.Value["alpha3"].ToString(),
                        ID = token.Value["id"].ToString()
                    })
                    .OrderBy(x => x.ID)
                    .ToList();
            }
            catch (Exception ex) {
                var dialog = new MessageDialog(Translation.GetTranslation("CheckInternetConnectionMessage"),
                    Translation.GetTranslation("CheckInternetConnectionTitle"));
                dialog.Commands.Add(new UICommand(Translation.GetTranslation("YesLabel")));
                dialog.ShowAsync();
            }
            return new List<Country>();
        }

        public async Task<double> GetCurrencyRatio(string currencyFrom, string currencyTo) {
            string currencyFromTo = string.Format("{0}-{1}", currencyFrom.ToUpper(), currencyTo.ToUpper());
            string url = string.Format(CURRENCY_SERVICE_URL, currencyFromTo);

            string jsonString = await _jsonService.GetJsonFromService(url);
            jsonString = jsonString.Replace(currencyFromTo, "Conversion");

            return ParseToExchangeRate(jsonString);
        }

        private double ParseToExchangeRate(string jsonString) {
            try {
                var typeExample =
                    new {
                        Conversion = new {
                            val = ""
                        }
                    };

                var currency = JsonConvert.DeserializeAnonymousType(jsonString, typeExample);
                //use US culture info for parsing, since service uses us format
                return double.Parse(currency.Conversion.val, new CultureInfo("en-us"));
            }
            catch (Exception ex) {
                InsightHelper.Report(ex);
            }
            return 1;
        }
    }
}