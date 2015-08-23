using SQLite.Net;

namespace MoneyManager.Foundation.OperationContracts
{
    public interface IDbHelper
    {
        SQLiteConnection GetSqlConnection();
        void CreateDatabase();
    }
}
