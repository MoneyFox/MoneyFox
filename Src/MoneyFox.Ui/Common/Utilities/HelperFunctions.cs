namespace MoneyFox.Ui.Common.Utilities;

using System.Globalization;

/// <summary>
///     Utility methods
/// </summary>
public static class HelperFunctions
{
    /// <summary>
    ///     Returns the decimal converted to a string in a proper format for this culture.
    /// </summary>
    /// <param name="value">decimal who shall be converted</param>
    /// <returns>Formated string.</returns>
    public static string FormatLargeNumbers(decimal value)
    {
        return value.ToString(format: "N2", provider: CultureInfo.CurrentCulture);
    }
}
