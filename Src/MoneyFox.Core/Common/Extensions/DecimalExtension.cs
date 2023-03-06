namespace MoneyFox.Core.Common.Extensions;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

public static class DecimalExtension
{
    private static IReadOnlyDictionary<string, CultureInfo> IsoCurrenciesToACultureMap
        => CultureInfo.GetCultures(CultureTypes.SpecificCultures)
            .Select(c => new { c, new RegionInfo(c.Name).ISOCurrencySymbol })
            .GroupBy(x => x.ISOCurrencySymbol)
            .ToDictionary(keySelector: g => g.Key, elementSelector: g => g.First().c, comparer: StringComparer.OrdinalIgnoreCase);

    public static string FormatCurrency(this decimal amount, string currencyCode)
    {
        return IsoCurrenciesToACultureMap.TryGetValue(key: currencyCode, out var culture)
            ? string.Format(provider: culture, format: "{0:C}", arg0: amount)
            : amount.ToString("0.00");
    }
}
