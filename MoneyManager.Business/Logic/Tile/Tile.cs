#region

using System;
using System.Threading.Tasks;
using Windows.UI.StartScreen;

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
    }
}