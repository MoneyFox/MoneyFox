namespace MoneyFox.Foundation.Interfaces
{
    public interface ITileManager
    {
        bool Exists(TyleType type);

        void CreateTile(TyleType type);

        void RemoveTile(TyleType type);
    }
}