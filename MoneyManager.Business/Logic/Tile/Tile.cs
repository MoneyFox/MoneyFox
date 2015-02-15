#region

using System;
using System.Threading.Tasks;
using Windows.UI.StartScreen;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Helper;
using MoneyManager.Foundation.OperationContracts;

#endregion

namespace MoneyManager.Business.Logic.Tile {
    public abstract class Tile {
        protected bool Exists(string id) {
            return SecondaryTile.Exists(id);
        }

        protected async Task Create(SecondaryTile secondTile) {
            secondTile.VisualElements.ShowNameOnSquare150x150Logo = true;
            await secondTile.RequestCreateAsync();
        }

        protected async Task Remove(SecondaryTile secondTile) {
            await secondTile.RequestDeleteAsync();
        }

        public static void UpdateMainTile()
        {
            var cashFlow = StatisticLogic.GetMonthlyCashFlow();

            ServiceLocator.Current.GetInstance<IUserNotification>()
                .UpdateMainTile(cashFlow[0].Value, cashFlow[1].Value, cashFlow[2].Value);
        }


    }
}