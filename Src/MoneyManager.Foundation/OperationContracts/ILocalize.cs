using System.Globalization;

namespace MoneyManager.Foundation.OperationContracts
{
    public interface ILocalize
    {
        CultureInfo GetCurrentCultureInfo();
    }
}