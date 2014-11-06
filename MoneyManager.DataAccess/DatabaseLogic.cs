using MoneyManager.DataAccess.Model;

namespace MoneyManager.DataAccess
{
    public class DatabaseLogic
    {
        public static void CreateDatabase()
        {
            using (var dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                dbConn.CreateTable<Account>();
                dbConn.CreateTable<FinancialTransaction>();
                dbConn.CreateTable<RecurringTransaction>();
                dbConn.CreateTable<Category>();
                dbConn.CreateTable<Country>();
            }
        }
    }
}