#region

using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Business;
using MoneyManager.Foundation.Model;
using SQLite.Net;

#endregion

namespace MoneyManager.DataAccess.WindowsPhone.Test {
    [TestClass]
    public class DatabaseLogicTest {
        [TestMethod]
        [TestCategory("Integration")]
        public void CreateDatabaseTest() {
            DatabaseLogic.CreateDatabase();

            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection()) {
                List<Account> temp1 = dbConn.Table<Account>().ToList();
                List<FinancialTransaction> temp2 = dbConn.Table<FinancialTransaction>().ToList();
                List<RecurringTransaction> temp3 = dbConn.Table<RecurringTransaction>().ToList();
                List<Category> temp4 = dbConn.Table<Category>().ToList();
            }
        }
    }
}