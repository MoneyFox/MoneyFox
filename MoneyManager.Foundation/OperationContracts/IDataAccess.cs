using System.Collections.Generic;

namespace MoneyManager.Foundation.OperationContracts
{
    internal interface IDataAccess<T>
    {
        void Save(T itemToSave);

        void Delete(T itemToDelete, bool suppressConfirmation = false);

        List<T> LoadList();

        void Update(T itemToUpdate);
    }
}