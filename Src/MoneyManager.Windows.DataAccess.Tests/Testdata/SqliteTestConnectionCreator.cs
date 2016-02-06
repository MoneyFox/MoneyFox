using System;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MvvmCross.Plugins.Sqlite;
using MvvmCross.Plugins.Sqlite.WindowsUWP;
using SQLite.Net;

namespace MoneyManager.Windows.DataAccess.Tests.Testdata
{
    public class SqliteTestConnectionCreator:ISqliteConnectionCreator
    {
        private readonly string dbname;
        private readonly MvxSqliteConnectionFactoryBase connectionFactory;

        public SqliteTestConnectionCreator()
        {
            connectionFactory = new WindowsSqliteConnectionFactory();
            dbname = DateTime.Now.ToString("O");
            CreateDb();
        }

        private void CreateDb()
        {
            using (var db = connectionFactory.GetConnection(dbname))
            {
                db.CreateTable<Account>();
                db.CreateTable<Payment>();
                db.CreateTable<RecurringPayment>();
                db.CreateTable<Category>();
            }
        }

        public SQLiteConnection GetConnection()
        {
            return connectionFactory.GetConnection(new SqLiteConfig(OneDriveAuthenticationConstants.DB_NAME, false));
        }
    }
}
