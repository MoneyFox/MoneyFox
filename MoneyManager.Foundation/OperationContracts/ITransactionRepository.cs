using System;
using System.Collections.Generic;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Foundation.OperationContracts {
    public interface ITransactionRepository : IRepository<FinancialTransaction> {
        IEnumerable<FinancialTransaction> GetUnclearedTransactions();
        IEnumerable<FinancialTransaction> GetUnclearedTransactions(DateTime date);
        IEnumerable<FinancialTransaction> GetRelatedTransactions(int accountId);
        List<FinancialTransaction> LoadRecurringList();
    }
}