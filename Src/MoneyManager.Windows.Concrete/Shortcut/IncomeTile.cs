using System;
using System.Threading.Tasks;
using Windows.UI.StartScreen;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces.Shotcuts;
using MoneyManager.Localization;

namespace MoneyManager.Windows.Concrete.Shortcut
{
    public class IncomeTile : Tile, IIncomeShortcut
    {
        public bool IsShortcutExisting => TileExists(Constants.ADD_INCOME_TILE_ID);

        public async Task CreateShortCut()
        {
            await Create(new SecondaryTile(
                Constants.ADD_INCOME_TILE_ID,
                Strings.AddIncomeLabel,
                "intake",
                new Uri("ms-appx:///Assets/IncomeTileIcon.png", UriKind.Absolute),
                TileSize.Default));
        }

        public async Task RemoveShortcut()
        {
            if (IsShortcutExisting)
            {
                await Remove(new SecondaryTile(Constants.ADD_INCOME_TILE_ID));
            }
        }
    }
}