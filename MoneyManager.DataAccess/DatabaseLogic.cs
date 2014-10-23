using MoneyManager.DataAccess;
using MoneyManager.DataAccess.Model;
using MoneyManager.Models;
using System.Threading.Tasks;

namespace MoneyManager.Src
{
    public class DatabaseLogic
    {
        public async static Task CreateDatabase()
        {
            var dbConn = SqlConnectionFactory.GetSqlConnection();

            dbConn.CreateTable<>()<Account>();
            dbConn.CreateTable<FinancialTransaction>();
            dbConn.CreateTable<RecurringTransaction>();
            dbConn.CreateTable<Group>();
            dbConn.CreateTable<Setting>();
        }
    }
}