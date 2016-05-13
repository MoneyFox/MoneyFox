using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MoneyFox.Shared.Interfaces;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;

namespace MoneyFox.Shared.DataAccess
{
    public abstract class AbstractDataAccess<T> : IDataAccess<T>
    {
        /// <summary>
        ///     Will insert the item to the database if not exists, otherwise will
        ///     update the existing
        /// </summary>
        /// <param name="itemToSave">item to save.</param>
        public bool SaveItem(T itemToSave)
        {
            try
            {
                SaveToDb(itemToSave);
            }
            catch (Exception ex)
            {
                Mvx.Trace(MvxTraceLevel.Error, ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        ///     Deletes the passed item from the database
        /// </summary>
        /// <param name="itemToDelete">Item to delete.</param>
        public bool DeleteItem(T itemToDelete)
        {
            try
            {
                DeleteFromDatabase(itemToDelete);
            }
            catch (Exception ex)
            {
                Mvx.Trace(MvxTraceLevel.Error, ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        ///     Loads all medicines and returns a list
        /// </summary>
        /// <returns>The list from db.</returns>
        public List<T> LoadList(Expression<Func<T, bool>> filter = null)
        {
            try
            {
                return GetListFromDb(filter);
            }
            catch (Exception ex)
            {
                Mvx.Trace(MvxTraceLevel.Error, ex.Message);
            }
            return new List<T>();
        }

        protected abstract void SaveToDb(T itemToAdd);
        protected abstract void DeleteFromDatabase(T payment);
        protected abstract List<T> GetListFromDb(Expression<Func<T, bool>> filter);
    }
}