using SQLite.Net;

namespace MoneyFox.Shared.Interfaces
{
    public interface ISqliteConnectionCreator
    {
        SQLiteConnection GetConnection();
    }
}