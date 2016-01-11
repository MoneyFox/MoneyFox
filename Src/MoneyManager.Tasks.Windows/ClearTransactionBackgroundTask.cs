using System;
using Windows.ApplicationModel.Background;
using Microsoft.ApplicationInsights;
using MoneyManager.Core.Manager;
using MoneyManager.Core.Repositories;
using MoneyManager.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Windows.Concrete.Shortcut;
using MvvmCross.Plugins.Sqlite.WindowsUWP;

namespace MoneyManager.Tasks.Windows
{
    public sealed class ClearTransactionBackgroundTask : IBackgroundTask
    {
        private readonly TransactionManager transactionManager;

        private readonly TelemetryClient telemetryClient;

        public ClearTransactionBackgroundTask()
        {
            telemetryClient = new TelemetryClient { InstrumentationKey = "ac915a37-36f5-436a-b85b-5a5617838bc8" };

            var sqliteConnectionCreator = new SqliteConnectionCreator(new WindowsSqliteConnectionFactory());

            transactionManager = new TransactionManager(
                new TransactionRepository(new TransactionDataAccess(sqliteConnectionCreator),
                    new RecurringTransactionDataAccess(sqliteConnectionCreator)),
                new AccountRepository(new AccountDataAccess(sqliteConnectionCreator)),
                null);
        }

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            try
            {
                telemetryClient.TrackEvent("Background Task started");

                transactionManager.ClearTransactions();
                Tile.UpdateMainTile();

                telemetryClient.TrackEvent("Background Task finished");
            } 
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
            }
        }
    }
}