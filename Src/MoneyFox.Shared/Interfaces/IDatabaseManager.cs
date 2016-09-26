using SQLite.Net;

namespace MoneyFox.Shared.Interfaces
{
    public interface IDatabaseManager
    {
        void CreateDatabase();

        SQLiteConnection GetConnection();
    }
}