using System;
using System.Threading.Tasks;
using Windows.UI.StartScreen;
using MoneyManager.Foundation.Interfaces.Shotcuts;
using MoneyManager.Localization;

namespace MoneyManager.Windows.Shortcut
{
    public class IncomeTile : Tile, IIncomeShortcut
    {
        public const string ID = "AddIncomeTile";

        public bool IsShortcutExisting => TileExists(ID);

        public async Task CreateShortCut()
        {
            await Create(new SecondaryTile(
                ID,
                Strings.AddIncomeLabel,
                "intake",
                new Uri("ms-appx:///Assets/IncomeTileIcon.png", UriKind.Absolute),
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