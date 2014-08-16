using BugSense;
using MoneyManager.OperationContracts;
using MoneyManager.Src;
using System;
using System.Threading.Tasks;
using Windows.UI.Popups;

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

        public async void Delete(T itemToDelete)
        {
            try
            {
                if (await IsDeletionConfirmed())
                {
                    DeleteFromDatabase(itemToDelete);
                }
            }
            catch (Exception ex)
            {
                BugSenseHandler.Instance.LogException(ex);
            }
        }

        private async Task<bool> IsDeletionConfirmed()
        {
            var dialog = new MessageDialog(Utilities.GetTranslation("DeleteEntryQuestionMessage"),
                Utilities.GetTranslation("DeleteQuestionMessage"));
            dialog.Commands.Add(new UICommand(Utilities.GetTranslation("YesLabel")));
            dialog.Commands.Add(new UICommand(Utilities.GetTranslation("NoLabel")));
            dialog.DefaultCommandIndex = 1;

            var result = await dialog.ShowAsync();

            return result.Label == Utilities.GetTranslation("YesLabel");
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