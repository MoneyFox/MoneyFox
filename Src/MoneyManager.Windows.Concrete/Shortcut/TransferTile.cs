using System;
using System.Threading.Tasks;
using Windows.UI.StartScreen;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces.Shotcuts;
using MoneyManager.Localization;

namespace MoneyManager.Windows.Concrete.Shortcut
{
    public class TransferTile : Tile, ITransferShortcut
    {
        public bool IsShortcutExisting => TileExists(Constants.ADD_TRANSFER_TILE_ID);

        public async Task CreateShortCut()
        {
            await Create(new SecondaryTile(
                Constants.ADD_TRANSFER_TILE_ID,
                Strings.AddTransferLabel,
                "intake",
                new Uri("ms-appx:///Assets/TransferTileIcon.png", UriKind.Absolute),
                TileSize.Default));
        }

        public async Task RemoveShortcut()
        {
            if (IsShortcutExisting)
            {
                await Remove(new SecondaryTile(Constants.ADD_TRANSFER_TILE_ID));
            }
        }
    }
}