using MoneyTracker.Models;
using System.Threading.Tasks;

namespace MoneyTracker.Src
{
    public class DatabaseHelper
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