using MoneyManager.DataAccess.Model;
using SQLite.Net;

namespace MoneyManager.DataAccess
{
    public class DatabaseLogic
    {
        public static void CreateDatabase()
        {
            SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection();

            dbConn.CreateTable<Account>();
            dbConn.CreateTable<FinancialTransaction>();
            dbConn.CreateTable<RecurringTransaction>();
            dbConn.CreateTable<Category>();
            dbConn.CreateTable<Country>();
        }
    }
}