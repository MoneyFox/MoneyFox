using System.Collections.Generic;
using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.Interfaces
{
    public interface IEndOfMonthManager
    {
        void CheckEndOfMonthBalanceForAccounts(IEnumerable<AccountViewModel> accounts);

        double GetTotalEndOfMonthBalance(IEnumerable<AccountViewModel> accounts);

        double GetEndOfMonthBalanceForAccount(AccountViewModel accountViewModel);
    }
}