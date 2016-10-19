using MoneyFox.Foundation;

namespace MoneyFox.Shared.Interfaces
{
    public interface ITileManager
    {
        bool Exists(TyleType type);

        void CreateTile(TyleType type);

        void RemoveTile(TyleType type);
    }
}