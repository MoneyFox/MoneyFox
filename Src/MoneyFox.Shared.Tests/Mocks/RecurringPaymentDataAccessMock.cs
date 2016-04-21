using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MoneyFox.Shared.Tests.Mocks
{
    public class RecurringPaymentDataAccessMock : IDataAccess<RecurringPayment>
    {
        public List<RecurringPayment> RecurringPaymentTestList = new List<RecurringPayment>();

        public void SaveItem(RecurringPayment itemToSave)
        {
            RecurringPaymentTestList.Add(itemToSave);
        }

        public void DeleteItem(RecurringPayment item)
        {
            if (RecurringPaymentTestList.Contains(item))
            {
                RecurringPaymentTestList.Remove(item);
            }
        }

        public List<RecurringPayment> LoadList(Expression<Func<RecurringPayment, bool>> filter = null) => RecurringPaymentTestList;
    }
}