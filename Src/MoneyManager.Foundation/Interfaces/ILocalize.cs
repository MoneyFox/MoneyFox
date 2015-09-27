using System.Globalization;

namespace MoneyManager.Foundation.Interfaces
{
    public interface ILocalize
    {
        CultureInfo GetCurrentCultureInfo();
    }
}