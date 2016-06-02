using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.Tests.Mocks
{
    public class RecurringPaymentDataAccessMock : IDataAccess<RecurringPayment>
    {
        public List<RecurringPayment> RecurringPaymentTestList = new List<RecurringPayment>();

        public bool SaveItem(RecurringPayment itemToSave)
        {
            RecurringPaymentTestList.Add(itemToSave);
            return true;
        }

        public bool DeleteItem(RecurringPayment item)
        {
            if (RecurringPaymentTestList.Contains(item))
            {
                RecurringPaymentTestList.Remove(item);
            }
            return true;
        }

        public List<RecurringPayment> LoadList(Expression<Func<RecurringPayment, bool>> filter = null)
            => RecurringPaymentTestList;
    }
}