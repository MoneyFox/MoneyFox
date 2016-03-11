using SQLite.Net;

namespace MoneyFox.Foundation.Interfaces
{
    public interface ISqliteConnectionFactory
    {
        SQLiteConnection GetConnection(string databaseName = null);
    }
}