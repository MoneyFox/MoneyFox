namespace MoneyFox.Core.Common.Extensions;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Domain;

public static class MoneyExtensions
{
    private static IReadOnlyDictionary<string, CultureInfo> IsoCurrenciesToACultureMap
        => CultureInfo.GetCultures(CultureTypes.SpecificCultures)
            .Select(c => new { c, new RegionInfo(c.Name).ISOCurrencySymbol })
            .GroupBy(x => x.ISOCurrencySymbol)
            .ToDictionary(keySelector: g => g.Key, elementSelector: g => g.First().c, comparer: StringComparer.OrdinalIgnoreCase);

    public static string FormatCurrency(this decimal amount, string currencyCode)
    {
        return IsoCurrenciesToACultureMap.TryGetValue(key: currencyCode, value: out var culture)
            ? string.Format(provider: culture, format: "{0:C}", arg0: amount)
            : amount.ToString("0.00");
    }

    public static string FormatCurrency(this Money money)
    {
        return IsoCurrenciesToACultureMap.TryGetValue(key: money.Currency.AlphaIsoCode, value: out var culture)
            ? string.Format(provider: culture, format: "{0:C}", arg0: money.Amount)
            : money.Amount.ToString("0.00");
    }
}
