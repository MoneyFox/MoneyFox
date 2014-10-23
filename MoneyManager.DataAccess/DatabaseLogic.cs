using MoneyManager.DataAccess;
using MoneyManager.DataAccess.Model;
using SQLite.Net;

namespace MoneyManager.DataAccess
{
    internal class DatabaseLogic
    {
        public static void CreateDatabase()
        {
            var dbConn = SqlConnectionFactory.GetSqlConnection();

            dbConn.CreateTable<Account>();
            dbConn.CreateTable<FinancialTransaction>();
            dbConn.CreateTable<RecurringTransaction>();
            dbConn.CreateTable<Group>();
            dbConn.CreateTable<Setting>();
        }
    }
}