using System.IO;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using SQLite.Net;
using SQLite.Net.Interop;

namespace MoneyManager.Core
{
    public class DbHelper : IDbHelper
    {
        private const string DATABASE_NAME = "moneyfox.sqlite";

        private readonly IDatabasePath path;
        private readonly ISQLitePlatform platform;

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
            return new SQLiteConnection(platform, Path.Combine(path.DbPath, DATABASE_NAME), false);
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