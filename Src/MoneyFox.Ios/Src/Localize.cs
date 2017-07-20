using System.Globalization;
using Foundation;

namespace MoneyFox.Ios
{
    /// <summary>
    ///     Localizatino helper
    /// </summary>
    public class Localize
    {
        /// <summary>
        ///     Returns the current culture.
        /// </summary>
        /// <returns>Current culture.</returns>
        public CultureInfo GetCurrentCultureInfo()
        {
            var netLanguage = "en";
            if (NSLocale.PreferredLanguages.Length > 0)
            {
                var pref = NSLocale.PreferredLanguages[0];
                netLanguage = pref.Replace("_", "-"); // turns es_ES into es-ES

                if (pref == "pt")
                {
                    // get the correct Brazilian language strings from the PCL RESX
                    //(note the local iOS folder is still "pt")
                    pref = "pt-BR";
                }
            }
            return new CultureInfo(netLanguage);
        }
    }
}