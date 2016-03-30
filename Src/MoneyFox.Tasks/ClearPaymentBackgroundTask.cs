using System;
using Windows.ApplicationModel.Background;
using Microsoft.ApplicationInsights;
using MoneyFox.Core.DataAccess;
using MoneyFox.Core.Manager;
using MoneyFox.Core.Repositories;
using MoneyFox.Core.Shortcut;
using MoneyFox.Foundation.Model;

namespace MoneyFox.Tasks
{
    public sealed class ClearPaymentBackgroundTask : IBackgroundTask
    {
        private readonly PaymentManager paymentManager;

        public ClearPaymentBackgroundTask()
        {
            var accountRepository = new AccountRepository(new GenericDataRepository<Account>());

            paymentManager = new PaymentManager(
                new PaymentRepository(new GenericDataRepository<Payment>(), 
                    new GenericDataRepository<RecurringPayment>(), 
                    accountRepository,
                    new CategoryRepository(new GenericDataRepository<Category>())),
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