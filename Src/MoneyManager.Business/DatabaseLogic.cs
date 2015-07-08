using MoneyManager.DataAccess;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Business
{
    public static class DatabaseLogic
    {
        public static void CreateDatabase()
        {
            using (var db = SqlConnectionFactory.GetSqlConnection())
            {
                db.CreateTable<Account>();
                db.CreateTable<FinancialTransaction>();
                db.CreateTable<RecurringTransaction>();
                db.CreateTable<Category>();
            }
        }
    }
}