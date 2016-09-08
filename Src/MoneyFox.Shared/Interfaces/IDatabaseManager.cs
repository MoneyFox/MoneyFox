using SQLite;

namespace MoneyFox.Shared.Interfaces
{
    public interface IDatabaseManager
    {
        void CreateDatabase();

        SQLiteConnection GetConnection();

        void MigrateDatabase();
    }
}