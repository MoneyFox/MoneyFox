using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BugSense;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Foundation
{
    internal abstract class AbstractDataAccess<T> : IDataAccess<T>
    {
        //private TotalBalanceViewModel TotalBalanceView
        //{
        //    get { return ServiceLocator.Current.GetInstance<TotalBalanceViewModel>(); }
        //}

        public void Save(T itemToSave)
        {
            try
            {
                SaveToDb(itemToSave);
                //TODO: Refactor
                //TotalBalanceView.UpdateBalance();
            }
            catch (Exception ex)
            {
                BugSenseHandler.Instance.LogException(ex);
            }
        }

        public async void Delete(T itemToDelete, bool suppressConfirmation = false)
        {
            try
            {
                if (suppressConfirmation || await IsDeletionConfirmed())
                {
                    DeleteFromDatabase(itemToDelete);
                    //TODO: Refactor
                    //TotalBalanceView.UpdateBalance();
                }
            }
            catch (Exception ex)
            {
                BugSenseHandler.Instance.LogException(ex);
            }
        }

        public List<T> LoadList()
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
                //TODO: Refactor
                //TotalBalanceView.UpdateBalance();
            }
            catch (Exception ex)
            {
                BugSenseHandler.Instance.LogException(ex);
            }
        }

        protected abstract void SaveToDb(T itemToAdd);

        protected abstract void DeleteFromDatabase(T itemToDelete);

        protected abstract List<T> GetListFromDb();

        protected abstract void UpdateItem(T itemToUpdate);

        private async Task<bool> IsDeletionConfirmed()
        {
            //TODO: refactor / move
            //var dialog = new MessageDialog(Utilities.GetTranslation("DeleteEntryQuestionMessage"),
            //    Utilities.GetTranslation("DeleteQuestionTitle"));
            //dialog.Commands.Add(new UICommand(Utilities.GetTranslation("YesLabel")));
            //dialog.Commands.Add(new UICommand(Utilities.GetTranslation("NoLabel")));
            //dialog.DefaultCommandIndex = 1;

            //var result = await dialog.ShowAsync();

            //return result.Label == Utilities.GetTranslation("YesLabel");

            return true;
        }
    }
}