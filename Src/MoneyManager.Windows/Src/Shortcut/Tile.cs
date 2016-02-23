using System;
using System.Threading.Tasks;
using Windows.UI.StartScreen;
using MoneyManager.Core.Extensions;
using MoneyManager.Core.StatisticDataProvider;
using MoneyManager.Foundation.Interfaces;
using MvvmCross.Platform;

namespace MoneyManager.Windows.Shortcut
{
    public abstract class Tile
    {
        protected bool TileExists(string id)
        {
            return SecondaryTile.Exists(id);
        }

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

        private static  void UpdateTile()
        {
            var cashFlow =
                new CashFlowDataProvider(Mvx.Resolve<IPaymentRepository>()).GetValues(
                    DateTime.Today.GetFirstDayOfMonth(),
                    DateTime.Today.GetLastDayOfMonth());

            Mvx.Resolve<IUserNotification>()
                .UpdateMainTile(cashFlow.Income.Label, cashFlow.Spending.Label, cashFlow.Revenue.Label);

        }

    }
}