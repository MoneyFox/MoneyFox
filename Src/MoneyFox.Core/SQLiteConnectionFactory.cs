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
        protected SqLiteConnectionFactory()
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

        public SQLiteConnection GetConnection(string databaseName)
        {
            return new SQLiteConnection(new SQLitePlatformWinRT(), GetDefaultBasePath(databaseName));
        }

        private string GetDefaultBasePath(string databaseName)
        {
            return Path.Combine(ApplicationData.Current.LocalFolder.Path, databaseName);
        }
    }
}
