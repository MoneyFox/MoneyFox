using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Models;
using MoneyManager.Src;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.WindowsPhone.Test.Src
{
    [TestClass]
    public class DatabaseHelperTest
    {
        [TestMethod]
        public async Task CreateDatabaseTest()
        {
            await DatabaseHelper.CreateDatabase();

            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                dbConn.Table<Account>().ToList();
                dbConn.Table<FinancialTransaction>().ToList();
                dbConn.Table<Setting>().ToList();
            }
        }
    }
}