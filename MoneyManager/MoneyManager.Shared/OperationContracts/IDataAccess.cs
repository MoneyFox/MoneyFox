namespace MoneyManager.OperationContracts
{
    public interface IDataAccess<T>
    {
        void Save(T itemToSave);

        void Delete(T itemToDelete, bool suppressConfirmation = false);

        void LoadList();

        void Update(T itemToUpdate);
    }
}