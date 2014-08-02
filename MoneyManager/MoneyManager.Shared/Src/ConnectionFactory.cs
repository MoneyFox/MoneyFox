using SQLite;
using System.IO;
using Windows.Storage;

namespace MoneyManager.Src
{
    public static class ConnectionFactory
    {
        private static readonly string _dbPath = Path.Combine(
            Path.Combine(ApplicationData.Current.LocalFolder.Path, "moneymanager.sqlite"));

        public static SQLiteConnection GetDbConnection()
        {
            return new SQLiteConnection(_dbPath);
        }

        public static SQLiteAsyncConnection GetAsyncDbConnection()
        {
            return new SQLiteAsyncConnection(_dbPath);
        }
    }
}