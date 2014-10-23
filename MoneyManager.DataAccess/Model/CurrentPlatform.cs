using SQLite.Net.Interop;

namespace MoneyManager.DataAccess.Model
{
    internal class CurrentPlatform
    {
        public ISQLitePlatform Platform { get; set; }
    }
}
