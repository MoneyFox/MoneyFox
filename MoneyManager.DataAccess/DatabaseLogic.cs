using System.Threading.Tasks;
using MoneyManager.DataAccess;
using MoneyManager.DataAccess.Model;
using SQLite.Net;

namespace MoneyManager.Src
{
    public class DatabaseLogic
    {
        public static async Task CreateDatabase()
        {
            SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection();

            dbConn.CreateTable<>() < Account > ()
            ;
            dbConn.CreateTable<FinancialTransaction>();
            dbConn.CreateTable<RecurringTransaction>();
            dbConn.CreateTable<Group>();
            dbConn.CreateTable<Setting>();
        }
    }
}