using MoneyFox.Core.DatabaseModels;

namespace MoneyFox.Core.Interfaces
{
    public interface IDefaultManager
    {
        Account GetDefaultAccount();
    }
}