using Windows.ApplicationModel.Background;
using MoneyManager.Core.Manager;
using MoneyManager.Core.Repositories;
using MoneyManager.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Windows.Core.Shortcut;
using MvvmCross.Plugins.Sqlite.WindowsUWP;

namespace MoneyManager.Tasks.Windows
{
    public sealed class ClearTransactionBackgroundTask : IBackgroundTask
    {
        private readonly TransactionManager transactionManager;

        public ClearTransactionBackgroundTask()
        {
            var sqliteConnectionCreator = new SqliteConnectionCreator(new WindowsSqliteConnectionFactory());

            transactionManager = new TransactionManager(
                new TransactionRepository(new TransactionDataAccess(sqliteConnectionCreator), new RecurringTransactionDataAccess(sqliteConnectionCreator)),
                new AccountRepository(new AccountDataAccess(sqliteConnectionCreator)), 
                null);
        }

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            transactionManager.ClearTransactions();
            Tile.UpdateMainTile();
        }
    }
}
