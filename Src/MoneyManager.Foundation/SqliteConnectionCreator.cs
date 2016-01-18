using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MvvmCross.Plugins.Sqlite;
using SQLite.Net;

namespace MoneyManager.Foundation
{
    public class SqliteConnectionCreator : ISqliteConnectionCreator
    {
        private readonly IMvxSqliteConnectionFactory connectionFactory;

        public SqliteConnectionCreator(IMvxSqliteConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;

            CreateDb();
        }

        /// <summary>
        ///     Creates the config and establishe the connection to the sqlite database.
        /// </summary>
        /// <returns>Established SQLiteConnection.</returns>
        public SQLiteConnection GetConnection()
        {
            return connectionFactory.GetConnection(new SqLiteConfig(OneDriveAuthenticationConstants.DB_NAME, false));
        }

        private void CreateDb()
        {
            using (var db = connectionFactory.GetConnection("moneyfox.sqlite"))
            {
                db.CreateTable<Account>();
                db.CreateTable<Payment>();
                db.CreateTable<RecurringPayment>();
                db.CreateTable<Category>();
            }
        }
    }
}