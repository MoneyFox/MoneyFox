using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Cheesebaron.MvxPlugins.Settings.WindowsUWP;
using MoneyFox.Business.Extensions;
using MoneyFox.Business.StatisticDataProvider;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.Infrastructure;
using MoneyFox.DataAccess.Repositories;
using MoneyFox.Service.DataServices;
using MoneyFox.Windows.Business;

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
            Debug.WriteLine("Start Clearing Payments.");

            try
            {
                var dbFactory = new DbFactory();
                paymentService = new PaymentService(new PaymentRepository(dbFactory), new UnitOfWork(dbFactory));

                await paymentService.SavePayments(await paymentService.GetUnclearedPayments(DateTime.Now));

                // We have to access the settings object here directly without the settings helper since this thread is executed independently.
                if (new WindowsUwpSettings().GetValue(SHOW_CASH_FLOW_ON_MAIN_TILE_KEYNAME, true))
                {
                    UpdateMainTile();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("There was an error in Clearing Payment.");
                Debug.Write(ex);
            }
            finally
            {
                deferral.Complete();
            }
        }

        private async void UpdateMainTile()
        {
            var cashFlow =
                await new CashFlowDataProvider(paymentService)
                    .GetCashFlow(DateTime.Today.GetFirstDayOfMonth(), DateTime.Today.GetLastDayOfMonth());

            new TileUpdateService().UpdateMainTile(cashFlow[0].Label, cashFlow[1].Label, cashFlow[2].Label);
        }
    }
}