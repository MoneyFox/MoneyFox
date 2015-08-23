using System;
using MoneyManager.Foundation.OperationContracts;
using SQLite.Net;

namespace MoneyManager.Core.Tests.Stubs
{
    public class DbHelperStub : IDbHelper
    {
        public SQLiteConnection GetSqlConnection()
        {
            throw new NotImplementedException();
        }

        public void CreateDatabase()
        {
            throw new NotImplementedException();
        }
    }
}