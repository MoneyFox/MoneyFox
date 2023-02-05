namespace MoneyFox.Core.Common.Helpers;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

public static class CurrencyHelper
{
    public static IReadOnlyDictionary<string, CultureInfo> IsoCurrenciesToACultureMap => CultureInfo.GetCultures(CultureTypes.SpecificCultures)
        .Select(c => new { c, new RegionInfo(c.Name).ISOCurrencySymbol })
        .GroupBy(x => x.ISOCurrencySymbol)
        .ToDictionary(keySelector: g => g.Key, elementSelector: g => g.First().c, comparer: StringComparer.OrdinalIgnoreCase);
}
