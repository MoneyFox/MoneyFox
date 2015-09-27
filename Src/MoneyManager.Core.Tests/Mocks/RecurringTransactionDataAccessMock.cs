using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.Tests.Mocks
{
    public class RecurringTransactionDataAccessMock : IDataAccess<RecurringTransaction>
    {
        public List<RecurringTransaction> RecurringTransactionTestList = new List<RecurringTransaction>();

        public void SaveItem(RecurringTransaction itemToSave)
        {
            RecurringTransactionTestList.Add(itemToSave);
        }

        public void DeleteItem(RecurringTransaction item)
        {
            if (RecurringTransactionTestList.Contains(item))
            {
                RecurringTransactionTestList.Remove(item);
            }
        }

        public List<RecurringTransaction> LoadList(Expression<Func<RecurringTransaction, bool>> filter = null)
        {
            return new List<RecurringTransaction>();
        }
    }
}