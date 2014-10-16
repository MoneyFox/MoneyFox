using BugSense;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoneyManager.Src
{
    public sealed class RecurringTransactionHelper
    {
        private RecurringTransactionDataAccess RecurringTransactionData
        {
            get { return ServiceLocator.Current.GetInstance<RecurringTransactionDataAccess>(); }
        }

        private IEnumerable<RecurringTransaction> AllRecurringTransactions
        {
            get { return ServiceLocator.Current.GetInstance<RecurringTransactionDataAccess>().AllRecurringTransactions; }
        }

        private TransactionDataAccess TransactionData
        {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>(); }
        }

        public async void CheckForRecurringTransactions()
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
            var transactions = TransactionData.LoadRecurringList();

            foreach (var recTrans in AllRecurringTransactions)
            {
                var relTransaction = transactions.Where(x => x.ReccuringTransactionId == recTrans.Id)
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
                case (int)TransactionRecurrence.Weekly:
                    return (relTransaction.Date - DateTime.Now).Days >= 7;

                case (int)TransactionRecurrence.Monthly:
                    return relTransaction.Date.Month != DateTime.Now.Month;
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

            TransactionData.SaveToDb(newTransaction, true);
        }
    }
}