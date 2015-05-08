using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Business.WindowsPhone.Test.Mocks {
    public class RecurringTransactionDataAccessMock : IDataAccess<RecurringTransaction> {
        public List<RecurringTransaction> RecurringTransactionTestList = new List<RecurringTransaction>();

        public void Save(RecurringTransaction itemToSave) {
            RecurringTransactionTestList.Add(itemToSave);
        }

        public void Delete(RecurringTransaction item) {
            if (RecurringTransactionTestList.Contains(item)) {
                RecurringTransactionTestList.Remove(item);
            }
        }

        public List<RecurringTransaction> LoadList(Expression<Func<RecurringTransaction, bool>> filter = null) {
            return new List<RecurringTransaction>();
        }
    }
}