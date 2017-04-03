using System;
using Windows.ApplicationModel.Background;
using Cheesebaron.MvxPlugins.Settings.WindowsUWP;
using MoneyFox.Business.Extensions;
using MoneyFox.Business.Manager;
using MoneyFox.Business.StatisticDataProvider;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.Repositories;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Windows.Business;
using MvvmCross.Plugins.File.WindowsCommon;
using MvvmCross.Plugins.Sqlite.WindowsUWP;
using Autofac;

namespace MoneyFox.Windows.Tasks
{
    public sealed class ClearPaymentTask : IBackgroundTask
    {
        private const string SHOW_CASH_FLOW_ON_MAIN_TILE_KEYNAME = "ShowCashFlowOnMainTile";

        private IPaymentManager paymentManager;
        private IPaymentRepository paymentRepository;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();

            try
            {
                MapperConfiguration.Setup();

                var dbManager = new DatabaseManager(new WindowsSqliteConnectionFactory(),
                    new MvxWindowsCommonFileStore());

                paymentRepository = new PaymentRepository(dbManager);

                paymentManager = new PaymentManager(paymentRepository,
                    new AccountRepository(dbManager),
                    new RecurringPaymentRepository(dbManager),
                    null);

                ClearPayments();
                PaymentRepository.IsCacheMarkedForReload = true;

                // We have to access the settings object here directly without the settings helper since this thread is executed independently.
                if (new WindowsUwpSettings().GetValue(SHOW_CASH_FLOW_ON_MAIN_TILE_KEYNAME, true))
                {
                    UpdateMainTile();
                }
            }
            finally
            {
                deferral.Complete();
            }
        }

        private void ClearPayments()
        {
            paymentManager.ClearPayments();
        }

        private void UpdateMainTile()
        {
            var cashFlow =
                new CashFlowDataProvider(paymentRepository)
                    .GetCashFlow(DateTime.Today.GetFirstDayOfMonth(), DateTime.Today.GetLastDayOfMonth());

            new TileUpdateService().UpdateMainTile(cashFlow[0].Label, cashFlow[1].Label, cashFlow[2].Label);
        }
    }
}