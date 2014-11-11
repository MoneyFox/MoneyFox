#region

using System;
using Windows.ApplicationModel.Background;
using BugSense;
using MoneyManager.Business.Logic;

#endregion

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