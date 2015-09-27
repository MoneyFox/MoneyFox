using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.Tests.Mocks
{
    public class AccountDataAccessMock : IDataAccess<Account>
    {
        public List<Account> AccountTestList = new List<Account>();

        public void SaveItem(Account itemToSave)
        {
            AccountTestList.Add(itemToSave);
        }

        public void DeleteItem(Account item)
        {
            if (AccountTestList.Contains(item))
            {
                AccountTestList.Remove(item);
            }
        }

        public List<Account> LoadList(Expression<Func<Account, bool>> filter = null)
        {
            return new List<Account>();
        }
    }
}