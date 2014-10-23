using System.Collections.Generic;

namespace MoneyManager.Foundation.OperationContracts
{
    internal interface IDataAccess<T>
    {
        void Save(T itemToSave);

        void Delete(T itemToDelete);

        void LoadList();

        void Update(T itemToUpdate);
    }
}