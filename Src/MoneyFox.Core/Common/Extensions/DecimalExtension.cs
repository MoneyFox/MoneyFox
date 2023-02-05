namespace MoneyFox.Core.Common.Extensions;

using Helpers;

public static class DecimalExtension
{
    public static string FormatCurrency(this decimal amount, string currencyCode)
    {
        return CurrencyHelper.IsoCurrenciesToACultureMap.TryGetValue(key: currencyCode, value: out var culture)
            ? string.Format(provider: culture, format: "{0:C}", arg0: amount)
            : amount.ToString("0.00");
    }
}
