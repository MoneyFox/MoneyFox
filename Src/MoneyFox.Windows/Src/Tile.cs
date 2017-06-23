using System;
using MoneyFox.Business.Extensions;
using MoneyFox.Business.StatisticDataProvider;
using MoneyFox.Foundation.Interfaces;
using MvvmCross.Platform;

namespace MoneyFox.Windows
{
    public abstract class Tile
    {
        /// <summary>
        ///     Get's the payment data from the Paymentrepository and refreshs the live tile with these information.
        /// </summary>
        public static async void UpdateMainTile()
        {
            // TODO: Refactor to non static and use injections + unit tests
            var cashFlow = await Mvx.Resolve<CashFlowDataProvider>()
                              .GetCashFlow(DateTime.Today.GetFirstDayOfMonth(), DateTime.Today.GetLastDayOfMonth());

            Mvx.Resolve<ITileUpdateService>()
                .UpdateMainTile(cashFlow[0].Label, cashFlow[1].Label, cashFlow[2].Label);
        }
    }
}