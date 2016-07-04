using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.Tests.Mocks
{
    public class AccountDataAccessMock : IDataAccess<Account>
    {
        public List<Account> AccountTestList = new List<Account>();

        public bool SaveItem(Account itemToSave)
        {
            AccountTestList.Add(itemToSave);
            return true;
        }

        public bool DeleteItem(Account item)
        {
            if (AccountTestList.Contains(item))
            {
                AccountTestList.Remove(item);
            }
            return true;
        }

        public List<Account> LoadList(Expression<Func<Account, bool>> filter = null) => AccountTestList;
    }
}