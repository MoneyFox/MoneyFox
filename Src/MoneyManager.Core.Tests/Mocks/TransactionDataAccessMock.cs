using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.Tests.Mocks
{
    public class TransactionDataAccessMock : IDataAccess<Payment>
    {
        public List<Payment> FinancialTransactionTestList = new List<Payment>();

        public void SaveItem(Payment itemToSave)
        {
            FinancialTransactionTestList.Add(itemToSave);
        }

        public void DeleteItem(Payment item)
        {
            if (FinancialTransactionTestList.Contains(item))
            {
                FinancialTransactionTestList.Remove(item);
            }
        }

        public List<Payment> LoadList(Expression<Func<Payment, bool>> filter = null)
        {
            return new List<Payment>();
        }
    }
}