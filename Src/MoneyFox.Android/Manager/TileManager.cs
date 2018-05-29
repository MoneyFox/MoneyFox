using System;
using System.Threading.Tasks;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces;

namespace MoneyFox.Droid.Manager
{
    public class TileManager : ITileManager
    {
        public bool Exists(TyleType type)
        {
            throw new PlatformNotSupportedException();
        }

        public void CreateTile(TyleType type)
        {
            throw new PlatformNotSupportedException();
        }

        public void RemoveTile(TyleType type)
        {
            throw new PlatformNotSupportedException();
        }

        public Task<bool> DoNavigation(string tileId)
        {
            throw new PlatformNotSupportedException();
        }
    }
}