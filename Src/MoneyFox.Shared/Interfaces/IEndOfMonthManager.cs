using System.Collections.Generic;
using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.Interfaces
{
    public interface IEndOfMonthManager
    {
        void CheckEndOfMonthBalanceForAccounts(IEnumerable<Account> accounts);

        double GetTotalEndOfMonthBalance(IEnumerable<Account> accounts);

        double GetEndOfMonthBalanceForAccount(Account account);
    }
}