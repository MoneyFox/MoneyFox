using System;
using System.Threading.Tasks;
using Windows.UI.StartScreen;
using Cirrious.CrossCore;
using MoneyManager.Core.Manager;
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
            var cashFlow = Mvx.Resolve<StatisticManager>().GetMonthlyCashFlow();
            Mvx.Resolve<IUserNotification>().UpdateMainTile(cashFlow[0].Label, cashFlow[1].Label, cashFlow[2].Label);
        }
    }
}