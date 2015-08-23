using System.IO;
using Windows.Storage;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Windows.Core.Tests.Helper
{
    public class TestDatabasePath : IDatabasePath
    {
        /// <summary>
        ///     Provides the platform specific database path for test purpose
        /// </summary>
        public string DbPath => Path.Combine(ApplicationData.Current.LocalFolder.Path, "moneyfoxTest.sqlite");
    }
}