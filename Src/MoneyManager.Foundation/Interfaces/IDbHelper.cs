using SQLite.Net;

namespace MoneyManager.Foundation.Interfaces
{
    public interface IDbHelper
    {
        /// <summary>
        ///     Returns an connection to the database
        /// </summary>
        /// <returns>Established Sqlite coonnection.</returns>
        SQLiteConnection GetSqlConnection();

        /// <summary>
        ///     Creates the database.
        /// </summary>
        void CreateDatabase();
    }
}