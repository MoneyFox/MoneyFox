using MoneyManager.Foundation.OperationContracts;
using SQLite.Net;

namespace MoneyManager.Core.Tests.Stubs
{
    public class DbHelperStub : IDbHelper
    {
        public SQLiteConnection GetSqlConnection()
        {
            throw new System.NotImplementedException();
        }

        public void CreateDatabase()
        {
            throw new System.NotImplementedException();
        }
    }
}
