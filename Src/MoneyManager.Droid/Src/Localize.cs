using System.Globalization;
using Java.Util;
using MoneyManager.Foundation.Interfaces;

namespace MoneyManager.Droid
{
    public class Localize : ILocalize
    {
        public CultureInfo GetCurrentCultureInfo()
        {
            var androidLocale = Locale.Default;
            var netLanguage = androidLocale.ToString().Replace("_", "-"); // turns pt_BR into pt-BR
            return new CultureInfo(netLanguage);
        }
    }
}