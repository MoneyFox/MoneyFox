using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MoneyFox.Shared.Tests.Mocks
{
    public class PaymentDataAccessMock : IDataAccess<Payment>
    {
        public List<Payment> PaymentTestList = new List<Payment>();

        public void SaveItem(Payment itemToSave)
        {
            PaymentTestList.Add(itemToSave);
        }

        public void DeleteItem(Payment item)
        {
            if (PaymentTestList.Contains(item))
            {
                PaymentTestList.Remove(item);
            }
        }

        public List<Payment> LoadList(Expression<Func<Payment, bool>> filter = null) => PaymentTestList;
    }
}