using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using SQLite.Net;
using SQLite.Net.Interop;

namespace MoneyManager.Core
{
    public class DbHelper : IDbHelper
    {
        private readonly ISQLitePlatform platform;
        private readonly IDatabasePath path;

        /// <summary>
        ///     Creates an instance of a DbHelper
        /// </summary>
        public DbHelper(ISQLitePlatform platform, IDatabasePath path)
        {
            this.platform = platform;
            this.path = path;

            CreateDatabase();
        }

        /// <summary>
        ///     Returns an SQLite Connection to access the database.
        /// </summary>
        /// <returns>Established database connection.</returns>
        public SQLiteConnection GetSqlConnection()
        {
            return new SQLiteConnection(platform, path.DbPath);
        }

        public void CreateDatabase()
        {
            using (var db = GetSqlConnection())
            {
                db.CreateTable<Account>();
                db.CreateTable<FinancialTransaction>();
                db.CreateTable<RecurringTransaction>();
                db.CreateTable<Category>();
            }
        }
    }
}