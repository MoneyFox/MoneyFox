using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Core.Logic
{
    public class RecurringTransactionLogic
    {
        public static void RemoveRecurringForTransactions(RecurringTransaction recTrans)
        {
            try
            {
                var relatedTrans =
                    transactionRepository.Data.Where(x => x.IsRecurring && x.ReccuringTransactionId == recTrans.Id);

                foreach (var transaction in relatedTrans)
                {
                    transaction.IsRecurring = false;
                    transaction.ReccuringTransactionId = null;
                    transactionRepository.Save(transaction);
                }
            }
            catch (Exception ex)
            {
                InsightHelper.Report(ex);
            }
        }

        public static void CheckRecurringTransactions()
        {
            RecurringTransactionData.LoadList();
            var transactionList = transactionRepository.LoadRecurringList();

            foreach (var recTrans in AllRecurringTransactions.Where(x => x.ChargedAccount != null))
            {
                var relTransaction = new FinancialTransaction();
                var trans = recTrans;
                var transcationList = transactionList.Where(
                    x => x.ReccuringTransactionId == trans.Id)
                    .OrderBy(x => x.Date);

                if (transcationList.Any())
                {
                    relTransaction = transcationList.Last();
                }

                if (CheckIfRepeatable(recTrans, relTransaction))
                {
                    SaveTransaction(recTrans);
                }
            }
        }

        private static bool CheckIfRepeatable(RecurringTransaction recTrans, FinancialTransaction relTransaction)
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

        private static void SaveTransaction(RecurringTransaction recurringTransaction)
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
                    AmountWithoutExchange = recurringTransaction.AmountWithoutExchange,
                    Currency = recurringTransaction.Currency,
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

        public static void Delete(RecurringTransaction recTransaction)
        {
            RecurringTransactionData.Delete(recTransaction);
            RemoveRecurringForTransactions(recTransaction);
        }

        public static RecurringTransaction GetRecurringFromFinancialTransaction(FinancialTransaction transaction)
        {
            return new RecurringTransaction
            {
                ChargedAccount = transaction.ChargedAccount,
                TargetAccount = transaction.TargetAccount,
                StartDate = transaction.Date,
                EndDate = addTransactionView.EndDate,
                IsEndless = addTransactionView.IsEndless,
                Amount = transaction.Amount,
                AmountWithoutExchange = transaction.AmountWithoutExchange,
                Currency = transaction.Currency,
                CategoryId = transaction.CategoryId,
                Type = transaction.Type,
                Recurrence = addTransactionView.Recurrence,
                Note = transaction.Note
            };
        }

        #region Properties

        private static IDataAccess<RecurringTransaction> RecurringTransactionData
        {
            get { return Mvx.Resolve<IDataAccess<RecurringTransaction>>(); }
        }

        private static ITransactionRepository transactionRepository
        {
            get { return Mvx.Resolve<ITransactionRepository>(); }
        }

        private static AddTransactionViewModel addTransactionView
        {
            get { return Mvx.Resolve<AddTransactionViewModel>(); }
        }

        private static IEnumerable<RecurringTransaction> AllRecurringTransactions
        {
            get { return Mvx.Resolve<IRepository<RecurringTransaction>>().Data; }
        }

        #endregion Properties
    }
}