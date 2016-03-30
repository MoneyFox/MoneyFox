using MoneyFox.Core.Model;
using MoneyFox.Foundation.Model;

namespace MoneyManager.Foundation.Interfaces
{
    public interface IDefaultManager
    {
        Account GetDefaultAccount();
    }
}