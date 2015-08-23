using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Core.Tests.Mocks
{
    public class TransactionDataAccessMock : IDataAccess<FinancialTransaction>
    {
        public List<FinancialTransaction> FinancialTransactionTestList = new List<FinancialTransaction>();

        public void Save(FinancialTransaction itemToSave)
        {
            FinancialTransactionTestList.Add(itemToSave);
        }

        public void Delete(FinancialTransaction item)
        {
            if (FinancialTransactionTestList.Contains(item))
            {
                FinancialTransactionTestList.Remove(item);
            }
        }

        public List<FinancialTransaction> LoadList(Expression<Func<FinancialTransaction, bool>> filter = null)
        {
            return new List<FinancialTransaction>();
        }
    }
}