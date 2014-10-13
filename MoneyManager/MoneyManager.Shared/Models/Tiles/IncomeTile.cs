using System;
using Windows.UI.StartScreen;
using MoneyManager.OperationContracts;
using MoneyManager.Src;

namespace MoneyManager.Models.Tiles
{
    public class IncomeTile : Tile, ISecondTile
    {
        public const string IncomeTileId = "AddIntakeTile";

        public bool Exists
        {
            get { return Exists(IncomeTileId); }
        }
        
        public async void Create()
        {
            await Create(new SecondaryTile(
                IncomeTileId,
                Utilities.GetTranslation("AddIncomeLabel"),
                "intake",
                new Uri("ms-appx:///Images/incomeTileIcon.png", UriKind.Absolute),
                TileSize.Default));
        }

        public void Remove()
        {
            throw new NotImplementedException();
        }
    }
}
