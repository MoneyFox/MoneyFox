namespace MoneyFox.Ui.Common.Utilities;

using System.Globalization;

/// <summary>
///     Utility methods
/// </summary>
public static class DecimalFormatter
{
    /// <summary>
    ///     Returns the decimal converted to a string in a proper format for this culture.
    /// </summary>
    /// <param name="value">decimal who shall be converted</param>
    /// <returns>Formatted string.</returns>
    public static string AsLargeNumber(decimal value)
    {
        return value.ToString(format: "N2", provider: CultureInfo.CurrentCulture);
    }
}
