using Windows.ApplicationModel.Background;
using Windows.Networking.Sockets;
using MoneyManager.Business.Src;

namespace MoneyManager.Tasks.TransactionsWp
{
    public sealed class TransactionTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            new ViewModelLocatorTask();
            RecurringTransactionLogic.CheckRecurringTransactions();
        }
    }
}