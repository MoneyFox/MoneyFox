namespace MoneyTracker.ViewModels
{
    public interface IDataAccess<T>
    {
        void Save(T itemToSave);

        void Delete(T itemToDelete);

        void LoadList();

        void Update(T itemToUpdate);
    }
}