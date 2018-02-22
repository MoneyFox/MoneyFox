using System.Globalization;
using Java.Util;

namespace MoneyFox.Droid
{
    public class Localize
    {
        public CultureInfo GetCurrentCultureInfo()
        {
            try
            {
                var androidLocale = Locale.Default;
                var netLanguage = androidLocale.ToString().Replace("_", "-"); // turns pt_BR into pt-BR
                return new CultureInfo(netLanguage);
            }
            catch(CultureNotFoundException)
            {
                return new CultureInfo("en");
            }
        }
    }
}