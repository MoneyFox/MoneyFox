using System;
using MoneyManager.Foundation;
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
                Id = transaction.ReccuringTransactionId,
                ChargedAccount = transaction.ChargedAccount,
                ChargedAccountId = transaction.ChargedAccount.Id,
                TargetAccount = transaction.TargetAccount,
                TargetAccountId = transaction.TargetAccount?.Id ?? 0,
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

        /// <summary>
        ///     Creates an Financial Transaction based on the Recurring transaction.
        /// </summary>
        /// <param name="recurringTransaction">The Recurring Transaction the new Transaction shall be based on.</param>
        /// <returns>The new created Financial Transaction</returns>
        public static FinancialTransaction GetFinancialTransactionFromRecurring(
            RecurringTransaction recurringTransaction)
        {
            var date = DateTime.Today;

            //If the transaction is monthly we want it on the same day of month again.
            if (recurringTransaction.Recurrence == (int) TransactionRecurrence.Monthly)
            {
                date = DateTime.Today.AddDays(recurringTransaction.StartDate.Day - DateTime.Today.Day);
            }

            return new FinancialTransaction
            {
                ChargedAccount = recurringTransaction.ChargedAccount,
                ChargedAccountId = recurringTransaction.ChargedAccountId,
                TargetAccount = recurringTransaction.TargetAccount,
                TargetAccountId = recurringTransaction.TargetAccountId,
                Date = date,
                IsRecurring = true,
                Amount = recurringTransaction.Amount,
                Category = recurringTransaction.Category,
                CategoryId = recurringTransaction.CategoryId,
                Type = recurringTransaction.Type,
                ReccuringTransactionId = recurringTransaction.Id,
                RecurringTransaction = recurringTransaction,
                Note = recurringTransaction.Note
            };
        }

        /// <summary>
        ///     Checks if the recurring Transaction is up for a repetition based on the passed Financial Transaction
        /// </summary>
        /// <param name="recTrans">RecurringTransaction to check.</param>
        /// <param name="relTransaction">Reference Transaction</param>
        /// <returns>True or False if the transaction have to be repeated.</returns>
        public static bool CheckIfRepeatable(RecurringTransaction recTrans, FinancialTransaction relTransaction)
        {
            switch (recTrans.Recurrence)
            {
                case (int) TransactionRecurrence.Daily:
                    return DateTime.Today.Date != relTransaction.Date.Date;

                case (int) TransactionRecurrence.DailyWithoutWeekend:
                    return DateTime.Today.Date != relTransaction.Date.Date
                           && DateTime.Today.DayOfWeek != DayOfWeek.Saturday
                           && DateTime.Today.DayOfWeek != DayOfWeek.Sunday;

                case (int) TransactionRecurrence.Weekly:
                    var days = DateTime.Now - relTransaction.Date;
                    return days.Days >= 7;

                case (int) TransactionRecurrence.Monthly:
                    return DateTime.Now.Month != relTransaction.Date.Month;

                case (int) TransactionRecurrence.Yearly:
                    return DateTime.Now.Year != relTransaction.Date.Year
                           && DateTime.Now.Month == relTransaction.Date.Month;

                default:
                    return false;
            }
        }
    }
}