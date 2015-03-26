using System.Collections.Generic;

namespace MoneyManager.Foundation.OperationContracts {
    public interface IDataAccess<T> {
        void Save(T itemToSave);

        void Delete(T itemToDelete);

        List<T> LoadList();
    }
}