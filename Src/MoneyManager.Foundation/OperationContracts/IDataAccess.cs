using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MoneyManager.Foundation.OperationContracts {
    public interface IDataAccess<T> {
        void Save(T itemToSave);
        void Delete(T itemToDelete);
        List<T> LoadList(Expression<Func<T, bool>> filter = null);
    }
}