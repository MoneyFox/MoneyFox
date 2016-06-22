using System.Globalization;

namespace MoneyFox.Shared.Interfaces {
    public interface ILocalize {
        /// <summary>
        ///     Returns the current culture in a standardised.
        /// </summary>
        /// <returns>Current Culutre.</returns>
        CultureInfo GetCurrentCultureInfo();
    }
}