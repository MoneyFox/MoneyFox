using MoneyManager.DataAccess.Model;
using MoneyManager.Models;
using System.Threading.Tasks;

namespace MoneyManager.Src
{
    internal class DatabaseHelper
    {
        public async static Task CreateDatabase()
        {
            var dbConn = ConnectionFactory.GetAsyncDbConnection();

            await dbConn.CreateTableAsync<Account>();
            await dbConn.CreateTableAsync<FinancialTransaction>();
            await dbConn.CreateTableAsync<RecurringTransaction>();
            await dbConn.CreateTableAsync<Group>();
            await dbConn.CreateTableAsync<Setting>();
        }
    }
}