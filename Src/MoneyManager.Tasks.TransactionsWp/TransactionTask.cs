using System;
using Windows.ApplicationModel.Background;
using MoneyManager.Business.Logic;
using MoneyManager.Foundation;

namespace MoneyManager.Tasks.TransactionsWp
{
    public sealed class TransactionTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            try
            {
                new BackgroundTaskViewModelLocator();
                RecurringTransactionLogic.CheckRecurringTransactions();
                await TransactionLogic.ClearTransactions();
            }
            catch (Exception ex)
            {
                InsightHelper.Report(ex);
            }
        }
    }
}