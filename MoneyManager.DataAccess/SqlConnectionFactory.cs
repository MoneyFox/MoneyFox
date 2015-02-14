#region

using System.IO;
using Windows.Storage;
using SQLite.Net;
using SQLite.Net.Interop;
using SQLite.Net.Platform.WinRT;

#endregion

namespace MoneyManager.DataAccess {
    public class SqlConnectionFactory {
        private static readonly string _dbPath =
            Path.Combine(ApplicationData.Current.LocalFolder.Path, "moneyfox.sqlite");

        public static SQLiteConnection GetSqlConnection() {
            return GetSqlConnection(new SQLitePlatformWinRT());
        }

        public static SQLiteConnection GetSqlConnection(ISQLitePlatform sqlitePlatform) {
            return new SQLiteConnection(sqlitePlatform, _dbPath);
        }
    }
}