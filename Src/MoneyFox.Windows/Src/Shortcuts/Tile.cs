using System;
using Microsoft.HockeyApp;
using MoneyFox.Business.Extensions;
using MoneyFox.Business.StatisticDataProvider;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Interfaces.Repositories;
using MvvmCross.Platform;

namespace MoneyFox.Windows.Shortcuts
{
    public abstract class Tile
    {
        /// <summary>
        ///     Get's the payment data from the Paymentrepository and refreshs the live tile with these information.
        /// </summary>
        public static void UpdateMainTile()
        {
            HockeyClient.Current.TrackEvent("Refresh Tile");

            var cashFlow =
                new CashFlowDataProvider(Mvx.Resolve<IPaymentRepository>())
                    .GetValues(DateTime.Today.GetFirstDayOfMonth(), DateTime.Today.GetLastDayOfMonth());

            Mvx.Resolve<ITileUpdateService>()
                .UpdateMainTile(cashFlow.Income.Label, cashFlow.Expense.Label, cashFlow.Revenue.Label);
        }
    }
}