using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.DataAccess;
using MoneyManager.DataAccess.Model;
using SQLite.Net;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.WindowsPhone.Test.Src
{
    [TestClass]
    internal class DatabaseHelperTest
    {
        [TestMethod]
        public async Task CreateDatabaseTest()
        {
            DatabaseLogic.CreateDatabase();

            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                dbConn.Table<Account>().ToList();
                dbConn.Table<FinancialTransaction>().ToList();
                dbConn.Table<RecurringTransaction>().ToList();
                dbConn.Table<Group>().ToList();
                dbConn.Table<Setting>().ToList();
            }
        }
    }
}