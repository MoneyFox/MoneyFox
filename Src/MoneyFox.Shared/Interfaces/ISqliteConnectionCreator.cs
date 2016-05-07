using SQLite.Net;

namespace MoneyFox.Shared.Interfaces
{
    public interface ISqliteConnectionCreator
    {
        void CreateDatabase();

        SQLiteConnection GetConnection();

        void MigrateDatabase();
    }
}