using System;
using System.Collections.Generic;
using System.Linq;
using BugSense;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.ViewModels;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;

namespace MoneyManager.Business
{
    internal class RecurringTransactionLogic
    {
        private RecurringTransactionDataAccess RecurringTransactionData
        {
            get { return ServiceLocator.Current.GetInstance<RecurringTransactionDataAccess>(); }
        }

        private static TransactionDataAccess transactionData
        {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>(); }
        }

        private static AddTransactionViewModel addTransactionView
        {
            get { return ServiceLocator.Current.GetInstance<AddTransactionViewModel>(); }
        }

        private IEnumerable<RecurringTransaction> AllRecurringTransactions
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RecurringTransactionDataAccess>().AllRecurringTransactions;
            }
        }

        public static void RemoveRecurringForTransactions(RecurringTransaction recTrans)
        {
            IEnumerable<FinancialTransaction> relatedTrans =
                transactionData.AllTransactions.Where(x => x.IsRecurring && x.ReccuringTransactionId == recTrans.Id);

            foreach (var transaction in relatedTrans)
            {
                transaction.IsRecurring = false;
                transaction.ReccuringTransactionId = null;
                transactionData.Update(transaction);
            }
        }

        public void CheckForRecurringTransactions()
        {
            try
            {
                RecurringTransactionData.LoadList();
                CheckIfIntervallIsReady();
            }
            catch (Exception ex)
            {
                BugSenseHandler.Instance.LogException(ex);
            }
        }

        private void CheckIfIntervallIsReady()
        {
            List<FinancialTransaction> transactions = transactionData.LoadRecurringList();

            foreach (RecurringTransaction recTrans in AllRecurringTransactions)
            {
                FinancialTransaction relTransaction = transactions.Where(x => x.ReccuringTransactionId == recTrans.Id)
                    .OrderBy(x => x.Date).Last();

                if (CheckIfRepeatable(recTrans, relTransaction))
                {
                    SaveTransaction(recTrans);
                }
            }
        }

        private bool CheckIfRepeatable(RecurringTransaction recTrans, FinancialTransaction relTransaction)
        {
            switch (recTrans.Recurrence)
            {
                case (int)TransactionRecurrence.Daily:
                    return DateTime.Now.Date != relTransaction.Date;

                case (int)TransactionRecurrence.Weekly:
                    TimeSpan days = DateTime.Now - relTransaction.Date;
                    return days.Days >= 7;

                case (int)TransactionRecurrence.Monthly:
                    return DateTime.Now.Month != relTransaction.Date.Month;

                case (int)TransactionRecurrence.Yearly:
                    return DateTime.Now.Year != relTransaction.Date.Year
                           && DateTime.Now.Month == relTransaction.Date.Month;
            }
            return false;
        }

        private void SaveTransaction(RecurringTransaction recurringTransaction)
        {
            var newTransaction = new FinancialTransaction
            {
                ChargedAccountId = recurringTransaction.ChargedAccountId,
                Date = DateTime.Now,
                IsRecurring = true,
                Amount = recurringTransaction.Amount,
                CurrencyCulture = recurringTransaction.Currency,
                CategoryId = recurringTransaction.CategoryId,
                Type = recurringTransaction.Type,
                ReccuringTransactionId = recurringTransaction.Id,
                Note = recurringTransaction.Note,
            };

            transactionData.SaveToDb(newTransaction, true);
        }

        public static RecurringTransaction GetRecurringFromFinancialTransaction(FinancialTransaction transaction)
        {
            return new RecurringTransaction
            {
                ChargedAccountId = transaction.ChargedAccountId,
                StartDate = transaction.Date,
                EndDate = addTransactionView.EndDate,
                IsEndless = addTransactionView.IsEndless,
                Amount = transaction.Amount,
                Currency = transaction.CurrencyCulture,
                CategoryId = transaction.CategoryId,
                Type = transaction.Type,
                Recurrence = addTransactionView.Recurrence,
                Note = transaction.Note,
            };
        }
    }
}