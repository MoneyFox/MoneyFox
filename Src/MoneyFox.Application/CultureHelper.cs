using System.Globalization;

namespace MoneyFox.Application
{
    public static class CultureHelper
    {
        public static CultureInfo CurrentCulture { get; set; } = CultureInfo.CurrentCulture;

        public static CultureInfo CurrentLocale { get; set; } = CultureInfo.CurrentCulture;
    }
}
