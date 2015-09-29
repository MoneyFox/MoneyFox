using Windows.Storage;
using MoneyManager.Foundation.Interfaces;

namespace MoneyManager.Windows.Core.Tests.Helper
{
    public class TestDatabasePath : IDatabasePath
    {
        /// <summary>
        ///     Provides the platform specific database path for test purpose
        /// </summary>
        public string DbPath => ApplicationData.Current.LocalFolder.Path;
    }
}