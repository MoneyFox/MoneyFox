using System;
using Windows.ApplicationModel.Background;
using Microsoft.ApplicationInsights;
using MoneyFox.Core;
using MoneyFox.Core.Manager;
using MoneyFox.Core.Repositories;
using MoneyFox.Core.Shortcut;
using MoneyFox.DataAccess;

namespace MoneyFOy.Tasks
{
    public sealed class ClearPaymentBackgroundTask : IBackgroundTask
    {
        private readonly PaymentManager paymentManager;

        public ClearPaymentBackgroundTask()
        {
            var sqliteConnectionFactory = new SqLiteConnectionFactory();
            var accountRepository = new AccountRepository(new AccountDataAccess(sqliteConnectionFactory));

            paymentManager = new PaymentManager(
                new PaymentRepository(new PaymentDataAccess(sqliteConnectionFactory), 
                    new RecurringPaymentDataAccess(sqliteConnectionFactory), 
                    accountRepository,
                    new CategoryRepository(new CategoryDataAccess(sqliteConnectionFactory))),
                accountRepository,
                null);
        }

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            try
            {
                paymentManager.ClearPayments();
                Tile.UpdateMainTile();
            }
            catch (Exception ex)
            {
                new TelemetryClient().TrackException(ex);
            }
        }
    }
}