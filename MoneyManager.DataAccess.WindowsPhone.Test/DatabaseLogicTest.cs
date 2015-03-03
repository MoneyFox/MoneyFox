#region

using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.DataAccess.Model;

#endregion

namespace MoneyManager.DataAccess.WindowsPhone.Test {
	[TestClass]
	public class DatabaseLogicTest {
		[TestMethod]
		public void CreateDatabaseTest() {
			DatabaseLogic.CreateDatabase();

			using (var dbConn = SqlConnectionFactory.GetSqlConnection()) {
				var temp1 = dbConn.Table<Account>().ToList();
				var temp2 = dbConn.Table<FinancialTransaction>().ToList();
				var temp3 = dbConn.Table<RecurringTransaction>().ToList();
				var temp4 = dbConn.Table<Category>().ToList();
			}
		}
	}
}