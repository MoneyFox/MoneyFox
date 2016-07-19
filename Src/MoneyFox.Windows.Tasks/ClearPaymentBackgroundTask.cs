using System;
using Windows.ApplicationModel.Background;
using MoneyFox.Shared;
using MoneyFox.Shared.Extensions;
using MoneyFox.Shared.Manager;
using MoneyFox.Shared.Repositories;
using MoneyFox.Shared.StatisticDataProvider;
using MvvmCross.Plugins.File.WindowsCommon;
using MvvmCross.Plugins.Sqlite.WindowsUWP;

namespace MoneyFox.Windows.Tasks
{
    public sealed class ClearPaymentBackgroundTask : IBackgroundTask
    {
        private UnitOfWork unitOfWork;
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            unitOfWork = new UnitOfWork(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                new MvxWindowsCommonFileStore()));

            ClearPayments();
            UpdateMainTile();
        }

        private void ClearPayments()
        {
            var paymentManager = new PaymentManager(unitOfWork, null);
            paymentManager.ClearPayments();
        }

        private void UpdateMainTile()
        {
            var cashFlow =
                new CashFlowDataProvider(unitOfWork)
                    .GetValues(DateTime.Today.GetFirstDayOfMonth(), DateTime.Today.GetLastDayOfMonth());

            new TileUpdateService().UpdateMainTile(cashFlow.Income.Label, cashFlow.Spending.Label, cashFlow.Revenue.Label);
        }
    }
}