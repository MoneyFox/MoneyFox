using System.Collections.Generic;
using MoneyFox.Foundation.DataModels;

namespace MoneyFox.Foundation.Interfaces
{
    public interface IEndOfMonthManager
    {
        void CheckEndOfMonthBalanceForAccounts(IEnumerable<AccountViewModel> accounts);

        double GetTotalEndOfMonthBalance(IEnumerable<AccountViewModel> accounts);

        double GetEndOfMonthBalanceForAccount(AccountViewModel accountViewModel);
    }
}