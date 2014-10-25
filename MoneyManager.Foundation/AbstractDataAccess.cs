using System;
using System.Threading.Tasks;
using BugSense;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Foundation
{
    internal abstract class AbstractDataAccess<T> : IDataAccess<T>
    {
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

        protected abstract void SaveToDb(T itemToAdd);

        protected abstract void DeleteFromDatabase(T itemToDelete);

        protected abstract void GetListFromDb();

        protected abstract void UpdateItem(T itemToUpdate);

        private async Task<bool> IsDeletionConfirmed()
        {
            //TODO: refactor / move
            //var dialog = new MessageDialog(Translation.GetTranslation("DeleteEntryQuestionMessage"),
            //    Translation.GetTranslation("DeleteQuestionTitle"));
            //dialog.Commands.Add(new UICommand(Translation.GetTranslation("YesLabel")));
            //dialog.Commands.Add(new UICommand(Translation.GetTranslation("NoLabel")));
            //dialog.DefaultCommandIndex = 1;

            //var result = await dialog.ShowAsync();

            //return result.Label == Translation.GetTranslation("YesLabel");

            return true;
        }
    }
}