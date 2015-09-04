using System;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.Helper
{
    public static class RecurringTransactionHelper
    {
        /// <summary>
        ///     Creates an recurring Transaction based on the Financial transaction.
        /// </summary>
        /// <param name="transaction">The financial transaction the reuccuring shall be based on.</param>
        /// <param name="isEndless">If the recurrence is infinite or not.</param>
        /// <param name="recurrence">How often the transaction shall be repeated.</param>
        /// <param name="enddate">Enddate for the recurring transaction if it's not endless.</param>
        /// <returns>The new created recurring transaction</returns>
        public static RecurringTransaction GetRecurringFromFinancialTransaction(FinancialTransaction transaction,
            bool isEndless,
            int recurrence,
            DateTime enddate = new DateTime())
        {
            return new RecurringTransaction
            {
                ChargedAccount = transaction.ChargedAccount,
                TargetAccount = transaction.TargetAccount,
                StartDate = transaction.Date,
                EndDate = enddate,
                IsEndless = isEndless,
                Amount = transaction.Amount,
                Category = transaction.Category,
                Type = transaction.Type,
                Recurrence = recurrence,
                Note = transaction.Note
            };
        }
    }
}
