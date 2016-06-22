using System;
using System.Threading.Tasks;
using Windows.UI.StartScreen;
using MoneyFox.Shared.Extensions;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.StatisticDataProvider;
using MvvmCross.Platform;

namespace MoneyFox.Windows.Shortcut
{
    public abstract class Tile
    {
        protected bool TileExists(string id) => SecondaryTile.Exists(id);

        protected async Task Create(SecondaryTile secondTile)
        {
            secondTile.VisualElements.ShowNameOnSquare150x150Logo = true;
            await secondTile.RequestCreateAsync();
        }

        protected async Task Remove(SecondaryTile secondTile)
        {
            await secondTile.RequestDeleteAsync();
        }

        /// <summary>
        ///     Will get the statistic manager and updates the main tile with the current cash flow.
        /// </summary>
        public static async void UpdateMainTile()
        {
            var task = Task.Run(() => UpdateTile());
            await task;
        }

        private static void UpdateTile()
        {
            var cashFlow =
                new CashFlowDataProvider(Mvx.Resolve<IPaymentRepository>()).GetValues(
                    DateTime.Today.GetFirstDayOfMonth(),
                    DateTime.Today.GetLastDayOfMonth());

            Mvx.Resolve<ITileUpdateService>()
                .UpdateMainTile(cashFlow.Income.Label, cashFlow.Spending.Label, cashFlow.Revenue.Label);
        }
    }
}