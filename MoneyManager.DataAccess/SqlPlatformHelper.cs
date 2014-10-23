using SQLite.Net.Interop;

namespace MoneyManager.DataAccess
{
    internal class SqlPlatformHelper
    {
        public static ISQLitePlatform GetCurrentPlatform()
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
