using System;
using System.Threading.Tasks;
using Windows.UI.StartScreen;
using MoneyFox.Foundation.Resources;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces.Shotcuts;

namespace MoneyFox.Core.Shortcut
{
    public class TransferTile : Tile, ITransferShortcut
    {
        public bool IsShortcutExisting => TileExists(Constants.Constants.ADD_TRANSFER_TILE_ID);

        public async Task CreateShortCut()
        {
            await Create(new SecondaryTile(
                Constants.Constants.ADD_TRANSFER_TILE_ID,
                Strings.AddTransferLabel,
                Constants.Constants.ADD_TRANSFER_TILE_ID,
                new Uri("ms-appx:///Assets/TransferTileIcon.png", UriKind.Absolute),
                TileSize.Default));
        }

        public async Task RemoveShortcut()
        {
            if (IsShortcutExisting)
            {
                await Remove(new SecondaryTile(Constants.Constants.ADD_TRANSFER_TILE_ID));
            }
        }
    }
}