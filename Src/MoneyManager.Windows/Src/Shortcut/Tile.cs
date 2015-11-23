using System;
using System.Threading.Tasks;
using Windows.UI.StartScreen;
using Cirrious.CrossCore;
using MoneyManager.Core.Extensions;
using MoneyManager.Core.StatisticProvider;
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
        public static void UpdateMainTile()
        {
            //TODO Refactor this so you don't create the CashFlowProvider here
            var cashFlow =
                new CashFlowProvider(Mvx.Resolve<ITransactionRepository>()).GetValues(
                    DateTime.Today.GetFirstDayOfMonth(),
                    DateTime.Today.GetLastDayOfMonth());

            Mvx.Resolve<IUserNotification>()
                .UpdateMainTile(cashFlow.Income.Label, cashFlow.Spending.Label, cashFlow.Revenue.Label);
        }
    }
}