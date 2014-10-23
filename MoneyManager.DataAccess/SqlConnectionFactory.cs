using System.IO;
using Windows.Storage;
using SQLite.Net;
using SQLite.Net.Interop;

namespace MoneyManager.DataAccess
{
    public class SqlConnectionFactory
    {
        private static readonly string _dbPath = Path.Combine(
            Path.Combine(ApplicationData.Current.LocalFolder.Path, "moneyfox.sqlite"));


        public static SQLiteConnection GetSqlConnection()
        {
            return GetSqlConnection(GetCurrentPlatform());
        }

        public static SQLiteConnection GetSqlConnection(ISQLitePlatform sqlitePlatform)
        {
            return new SQLiteConnection(sqlitePlatform, _dbPath);
        }

        private static ISQLitePlatform GetCurrentPlatform()
        {
#if WINDOWS_PHONE_APP
                return new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT()
#endif

#if WINDOWS_APP
                return new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT()
#endif
        }
    }
}