using System.Threading.Tasks;
using MoneyManager.DataAccess.Model;

namespace MoneyManager.Src
{
    internal class DatabaseHelper
    {
        public static async Task CreateDatabase()
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