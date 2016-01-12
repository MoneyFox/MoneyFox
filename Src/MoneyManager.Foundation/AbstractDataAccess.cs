using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.ApplicationInsights;
using MoneyManager.Foundation.Interfaces;
using Xamarin;

namespace MoneyManager.Foundation
{
    public abstract class AbstractDataAccess<T> : IDataAccess<T>
    {
        /// <summary>
        ///     Will insert the item to the database if not exists, otherwise will
        ///     update the existing
        /// </summary>
        /// <param name="itemToSave">item to save.</param>
        public void SaveItem(T itemToSave)
        {
            try
            {
                SaveToDb(itemToSave);
            }
            catch (Exception ex)
            {
                new TelemetryClient();
                Insights.Report(ex, Insights.Severity.Error);
            }
        }

        /// <summary>
        ///     Deletes the passed item from the database
        /// </summary>
        /// <param name="itemToDelete">Item to delete.</param>
        public void DeleteItem(T itemToDelete)
        {
            try
            {
                DeleteFromDatabase(itemToDelete);
            }
            catch (Exception ex)
            {
                Insights.Report(ex, Insights.Severity.Error);
            }
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
                Insights.Report(ex, Insights.Severity.Error);
            }
            return new List<T>();
        }

        protected abstract void SaveToDb(T itemToAdd);
        protected abstract void DeleteFromDatabase(T itemToDelete);
        protected abstract List<T> GetListFromDb(Expression<Func<T, bool>> filter);
    }
}