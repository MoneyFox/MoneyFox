using System;
using System.Collections.Generic;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Foundation.Interfaces
{
    public interface ITransactionRepository : IRepository<FinancialTransaction>
    {
        IEnumerable<FinancialTransaction> GetUnclearedTransactions();
        IEnumerable<FinancialTransaction> GetUnclearedTransactions(DateTime date);
        IEnumerable<FinancialTransaction> GetRelatedTransactions(Account account);
        IEnumerable<FinancialTransaction> LoadRecurringList(Func<FinancialTransaction, bool> filter = null);
    }
}