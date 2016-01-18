using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.Tests.Mocks
{
    public class RecurringTransactionDataAccessMock : IDataAccess<RecurringPayment>
    {
        public List<RecurringPayment> RecurringTransactionTestList = new List<RecurringPayment>();

        public void SaveItem(RecurringPayment itemToSave)
        {
            RecurringTransactionTestList.Add(itemToSave);
        }

        public void DeleteItem(RecurringPayment item)
        {
            if (RecurringTransactionTestList.Contains(item))
            {
                RecurringTransactionTestList.Remove(item);
            }
        }

        public List<RecurringPayment> LoadList(Expression<Func<RecurringPayment, bool>> filter = null)
        {
            return new List<RecurringPayment>();
        }
    }
}