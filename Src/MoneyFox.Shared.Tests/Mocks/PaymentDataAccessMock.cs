using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.Tests.Mocks
{
    public class PaymentDataAccessMock : IDataAccess<Payment>
    {
        public List<Payment> PaymentTestList = new List<Payment>();

        public bool SaveItem(Payment itemToSave)
        {
            PaymentTestList.Add(itemToSave);
            return true;
        }

        public bool DeleteItem(Payment item)
        {
            if (PaymentTestList.Contains(item))
            {
                PaymentTestList.Remove(item);
            }
            return true;
        }

        public List<Payment> LoadList(Expression<Func<Payment, bool>> filter = null) => PaymentTestList;
    }
}