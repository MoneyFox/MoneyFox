using System;
using Windows.ApplicationModel.Background;
using Cheesebaron.MvxPlugins.Settings.WindowsCommon;
using MoneyFox.Shared;
using MoneyFox.Shared.Extensions;
using MoneyFox.Shared.Manager;
using MoneyFox.Shared.Repositories;
using MoneyFox.Shared.StatisticDataProvider;
using MoneyFox.Windows.Business;
using MvvmCross.Plugins.File.WindowsCommon;
using MvvmCross.Plugins.Sqlite.WindowsUWP;

namespace MoneyFox.Windows.Tasks
{
    public sealed class ClearPaymentTask : IBackgroundTask
    {
        private const string SHOW_CASH_FLOW_ON_MAIN_TILE_KEYNAME = "ShowCashFlowOnMainTile";

        private UnitOfWork unitOfWork;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            unitOfWork = new UnitOfWork(new DatabaseManager(new WindowsSqliteConnectionFactory(),
                new MvxWindowsCommonFileStore()));

            ClearPayments();

            // We have to access the settings object here directly without the settings helper since this thread is executed independently.
            if (new WindowsCommonSettings().GetValue(SHOW_CASH_FLOW_ON_MAIN_TILE_KEYNAME, true))
            {
                UpdateMainTile();
            }
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