using System;
using System.Threading.Tasks;
using Windows.UI.StartScreen;
using MoneyManager.Foundation.Interfaces.Shotcuts;
using MoneyManager.Localization;

namespace MoneyManager.Windows.Shortcut
{
    public class SpendingTile : Tile, ISpendingShortcut
    {
        public const string ID = "AddSpendingTile";

        public bool IsShortcutExisting => TileExists(ID);

        public async Task CreateShortCut()
        {
            await Create(new SecondaryTile(
                ID,
                Strings.AddSpendingLabel,
                "intake",
                new Uri("ms-appx:///Assets/SpendingTileIcon.png", UriKind.Absolute),
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