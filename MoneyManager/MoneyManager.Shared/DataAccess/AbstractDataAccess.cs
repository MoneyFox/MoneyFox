using MoneyTracker.ViewModels;
using System;

namespace MoneyManager.DataAccess
{
    public abstract class AbstractDataAccess<T> : IDataAccess<T>
    {
        protected abstract void SaveToDb(T itemToAdd);

        protected abstract void DeleteFromDatabase(T itemToDelete);

        protected abstract void GetListFromDb();

        protected abstract void UpdateItem(T itemToUpdate);

        public void Save(T itemToSave)
        {
            try
            {
                SaveToDb(itemToSave);
            }
            catch (Exception ex)
            {
                BugSenseHandler.Instance.LogException(ex);
            }
        }

        public void Delete(T itemToDelete)
        {
            try
            {
                DeleteFromDatabase(itemToDelete);
            }
            catch (Exception ex)
            {
                BugSenseHandler.Instance.LogException(ex);
            }
        }

        public void LoadList()
        {
            try
            {
                GetListFromDb();
            }
            catch (Exception ex)
            {
                BugSenseHandler.Instance.LogException(ex);
            }
        }

        public void Update(T itemToUpdate)
        {
            try
            {
                UpdateItem(itemToUpdate);
            }
            catch (Exception ex)
            {
                BugSenseHandler.Instance.LogException(ex);
            }
        }
    }
}