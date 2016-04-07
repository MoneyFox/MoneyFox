using System.Globalization;
using MoneyFox.Shared.Interfaces;

namespace MoneyFox.Droid
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