using SQLite.Net;

namespace MoneyManager.Foundation.Interfaces
{
    public interface IDbHelper
    {
        SQLiteConnection GetSqlConnection();
        void CreateDatabase();
    }
}