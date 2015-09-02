using System;
using System.Collections.Generic;
using System.Linq;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Core.Logic
{
    public class RecurringTransactionLogic
    {
        private readonly ITransactionRepository transactionRepository;

        public RecurringTransactionLogic(ITransactionRepository transactionRepository)
        {
            this.transactionRepository = transactionRepository;
        }

        public void CheckRecurringTransactions()
        {
            var transactionList = transactionRepository.LoadRecurringList();

            foreach (var transaction in transactionRepository.LoadRecurringList(x => x.ChargedAccount != null))
            {
                var relTransaction = new FinancialTransaction();

                var financialTransactions = transactionList as IList<FinancialTransaction>;

                var transcationList = financialTransactions.Where(
                    x => x.ReccuringTransactionId == transaction.Id)
                    .OrderBy(x => x.Date);

                if (transcationList.Any())
                {
                    relTransaction = transcationList.Last();
                }

                if (CheckIfRepeatable(transaction.RecurringTransaction, relTransaction))
                {
                    SaveTransaction(transaction.RecurringTransaction);
                }
            }
        }

        private bool CheckIfRepeatable(RecurringTransaction recTrans, FinancialTransaction relTransaction)
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
            }
            return false;
        }

        private void SaveTransaction(RecurringTransaction recurringTransaction)
        {
            try
            {
                var date = DateTime.Now;

                if (recurringTransaction.Recurrence == (int) TransactionRecurrence.Monthly)
                {
                    date = DateTime.Now.AddDays(recurringTransaction.StartDate.Day - DateTime.Today.Day);
                }

                var newTransaction = new FinancialTransaction
                {
                    ChargedAccount = recurringTransaction.ChargedAccount,
                    TargetAccount = recurringTransaction.TargetAccount,
                    Date = date,
                    IsRecurring = true,
                    Amount = recurringTransaction.Amount,
                    CategoryId = recurringTransaction.CategoryId,
                    Type = recurringTransaction.Type,
                    ReccuringTransactionId = recurringTransaction.Id,
                    Note = recurringTransaction.Note
                };

                transactionRepository.Save(newTransaction);
            }
            catch (Exception ex)
            {
                InsightHelper.Report(ex);
            }
        }
    }
}