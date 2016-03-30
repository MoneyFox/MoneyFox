using System;
using System.Threading.Tasks;
using Windows.UI.StartScreen;
using MoneyFox.Core.Interfaces.Shotcuts;
using MoneyFox.Core.Resources;

namespace MoneyFox.Core.Shortcut
{
    public class IncomeTile : Tile, IIncomeShortcut
    {
        public bool IsShortcutExisting => TileExists(Constants.Constants.ADD_INCOME_TILE_ID);

        public async Task CreateShortCut()
        {
            await Create(new SecondaryTile(
                Constants.Constants.ADD_INCOME_TILE_ID,
                Strings.AddIncomeLabel,
                Constants.Constants.ADD_INCOME_TILE_ID,
                new Uri("ms-appx:///Assets/IncomeTileIcon.png", UriKind.Absolute),
                TileSize.Default));
        }

        public async Task RemoveShortcut()
        {
            if (IsShortcutExisting)
            {
                await Remove(new SecondaryTile(Constants.Constants.ADD_INCOME_TILE_ID));
            }
        }
    }
}