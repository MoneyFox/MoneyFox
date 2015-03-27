#region

using System;
using Windows.ApplicationModel.Background;
using MoneyManager.Business.Logic;
using Xamarin;

#endregion

namespace MoneyManager.Tasks.TransactionsWp {
    public sealed class TransactionTask : IBackgroundTask {
        public async void Run(IBackgroundTaskInstance taskInstance) {
            try {
                new BackgroundTaskViewModelLocator();
                RecurringTransactionLogic.CheckRecurringTransactions();
                await TransactionLogic.ClearTransactions();
            }
            catch (Exception ex) {
                Insights.Report(ex, ReportSeverity.Error);
            }
        }
    }
}