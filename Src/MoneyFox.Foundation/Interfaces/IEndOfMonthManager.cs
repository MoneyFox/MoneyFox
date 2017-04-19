using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyFox.Foundation.DataModels;

namespace MoneyFox.Foundation.Interfaces
{
    public interface IEndOfMonthManager
    {
        void CheckEndOfMonthBalanceForAccounts(IEnumerable<AccountViewModel> accounts);

        Task<double> GetTotalEndOfMonthBalance(IEnumerable<AccountViewModel> accounts);

        Task<double> GetEndOfMonthBalanceForAccount(AccountViewModel accountViewModel);
    }
}