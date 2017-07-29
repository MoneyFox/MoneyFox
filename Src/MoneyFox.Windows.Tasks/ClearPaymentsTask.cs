using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Cheesebaron.MvxPlugins.Settings.WindowsUWP;
using MoneyFox.Business.Extensions;
using MoneyFox.Business.StatisticDataProvider;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.Infrastructure;
using MoneyFox.DataAccess.Repositories;
using MoneyFox.Foundation.Constants;
using MoneyFox.Service.DataServices;

namespace MoneyFox.Windows.Tasks
{
    /// <summary>
    ///     Background task to periodically clear payments.
    /// </summary>
    public sealed class ClearPaymentsTask : IBackgroundTask
    {
        private const string SHOW_CASH_FLOW_ON_MAIN_TILE_KEYNAME = "ShowCashFlowOnMainTile";

        private IPaymentService paymentService;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            Debug.WriteLine("ClearPayment started");
            ApplicationContext.DbPath = DatabaseConstants.DB_NAME;

            try
            {
                var dbFactory = new DbFactory();

                paymentService = new PaymentService(new PaymentRepository(dbFactory), new UnitOfWork(dbFactory));

                var payments = await paymentService.GetUnclearedPayments(DateTime.Now);
                var unclearedPayments = payments.ToList();
                if (unclearedPayments.Any())
                {
                    await paymentService.SavePayments(unclearedPayments);
                }
            }
            finally
            {
                Debug.WriteLine("ClearPayment stopped.");
                deferral.Complete();
            }
        }
    }
}