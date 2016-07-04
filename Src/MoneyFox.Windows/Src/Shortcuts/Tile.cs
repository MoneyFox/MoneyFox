using System;
using System.Threading.Tasks;
using Windows.UI.StartScreen;
using Microsoft.HockeyApp;
using MoneyFox.Shared.Extensions;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.StatisticDataProvider;
using MvvmCross.Platform;

namespace MoneyFox.Windows.Shortcuts {
    public abstract class Tile {
        protected bool TileExists(string id) => SecondaryTile.Exists(id);

        protected async Task Create(SecondaryTile secondTile) {
            secondTile.VisualElements.ShowNameOnSquare150x150Logo = true;
            await secondTile.RequestCreateAsync();
        }

        protected async Task Remove(SecondaryTile secondTile) {
            await secondTile.RequestDeleteAsync();
        }

        /// <summary>
        ///     Get's the payment data from the Paymentrepository and refreshs the live tile with these information.
        /// </summary>
        public static void UpdateMainTile() {
            HockeyClient.Current.TrackEvent("Refresh Tile");

            var cashFlow =
                new CashFlowDataProvider(Mvx.Resolve<IPaymentRepository>())
                    .GetValues(DateTime.Today.GetFirstDayOfMonth(), DateTime.Today.GetLastDayOfMonth());

            Mvx.Resolve<ITileUpdateService>()
                .UpdateMainTile(cashFlow.Income.Label, cashFlow.Spending.Label, cashFlow.Revenue.Label);
        }
    }
}