#region

using MoneyManager.DataAccess.Model;
using SQLite.Net;

#endregion

namespace MoneyManager.DataAccess {
    public class DatabaseLogic {
        public static void CreateDatabase() {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection()) {
                dbConn.CreateTable<Account>();
                dbConn.CreateTable<FinancialTransaction>();
                dbConn.CreateTable<RecurringTransaction>();
                dbConn.CreateTable<Category>();
            }
        }
    }
}