using System;
using BugSense;
using MoneyManager.Business.Logic;
using Windows.ApplicationModel.Background;

namespace MoneyManager.Tasks.TransactionsWp
{
    public sealed class TransactionTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            try
            {
                new BackgroundTaskViewModelLocator();
                RecurringTransactionLogic.CheckRecurringTransactions();
                TransactionLogic.ClearTransactions();
            }
            catch (Exception ex)
            {
                BugSenseHandler.Instance.LogException(ex);
            }
        }
    }
}