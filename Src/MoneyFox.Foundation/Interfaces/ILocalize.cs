using System.Globalization;

namespace MoneyManager.Foundation.Interfaces
{
    public interface ILocalize
    {
        /// <summary>
        ///     Returns the current culture in a standardised.
        /// </summary>
        /// <returns>Current Culutre.</returns>
        CultureInfo GetCurrentCultureInfo();
    }
}