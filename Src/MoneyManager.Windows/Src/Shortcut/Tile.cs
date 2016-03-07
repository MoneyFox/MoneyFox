using System;
using System.Threading.Tasks;
using Windows.UI.StartScreen;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Core.Extensions;
using MoneyManager.Core.StatisticDataProvider;
using MoneyManager.Foundation.Interfaces;

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

        private static void UpdateTile()
        {
            var cashFlow =
                new CashFlowDataProvider(ServiceLocator.Current.GetInstance<IPaymentRepository>()).GetValues(
                    DateTime.Today.GetFirstDayOfMonth(),
                    DateTime.Today.GetLastDayOfMonth());

            ServiceLocator.Current.GetInstance<IUserNotification>()
                .UpdateMainTile(cashFlow.Income.Label, cashFlow.Spending.Label, cashFlow.Revenue.Label);
        }
    }
}