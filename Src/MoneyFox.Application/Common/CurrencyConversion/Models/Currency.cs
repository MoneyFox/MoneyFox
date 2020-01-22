using Newtonsoft.Json;

namespace MoneyFox.Application.Common.CurrencyConversion.Models
{
    public class Currency
    {
        [JsonProperty("currencyName")] public string CurrencyName { get; set; }

        [JsonProperty("currencySymbol")] public string CurrencySymbol { get; set; }

        [JsonProperty("id")] public string Id { get; set; }
    }
}
