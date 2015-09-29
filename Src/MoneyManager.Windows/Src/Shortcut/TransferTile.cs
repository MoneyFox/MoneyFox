using System;
using System.Threading.Tasks;
using Windows.UI.StartScreen;
using MoneyManager.Foundation.Interfaces.Shotcuts;
using MoneyManager.Localization;

namespace MoneyManager.Windows.Shortcut
{
    public class TransferTile : Tile, ITransferShortcut
    {
        public const string ID = "AddTransferTile";

        public bool IsShortcutExisting => TileExists(ID);

        public async Task CreateShortCut()
        {
            await Create(new SecondaryTile(
                ID,
                Strings.AddTransferLabel,
                "intake",
                new Uri("ms-appx:///Assets/TransferTileIcon.png", UriKind.Absolute),
                TileSize.Default));
        }

        public async Task RemoveShortcut()
        {
            if (IsShortcutExisting)
            {
                await Remove(new SecondaryTile(ID));
            }
        }
    }
}