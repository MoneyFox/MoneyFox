using System.IO;
using Windows.Storage;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Model;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;

namespace MoneyFox.Core
{
    public class SqLiteConnectionFactory : ISqliteConnectionFactory
    {
        public SqLiteConnectionFactory()
        {
            CreateDb();
        }

        private void CreateDb()
        {
            using (var db = GetConnection(OneDriveAuthenticationConstants.DB_NAME))
            {
                db.CreateTable<Account>();
                db.CreateTable<Payment>();
                db.CreateTable<RecurringPayment>();
                db.CreateTable<Category>();
            }
        }

        public SQLiteConnection GetConnection(string databaseName = null)
        {
            var dbName = databaseName ?? OneDriveAuthenticationConstants.DB_NAME;

            return new SQLiteConnection(new SQLitePlatformWinRT(), GetDefaultBasePath(dbName));
        }

        private string GetDefaultBasePath(string databaseName)
        {
            return Path.Combine(ApplicationData.Current.LocalFolder.Path, databaseName);
        }
    }
}
