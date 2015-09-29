using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MoneyManager.Foundation.Interfaces
{
    /// <summary>
    ///     Defines the basic Input / Output Operations for the database
    /// </summary>
    /// <typeparam name="T">Entity Type</typeparam>
    public interface IDataAccess<T>
    {
        /// <summary>
        ///     Updates or Inserts an item on the database
        /// </summary>
        /// <param name="itemToSave">Item of type T to save.</param>
        void SaveItem(T itemToSave);

        /// <summary>
        ///     Will Load the data from the database
        /// </summary>
        /// <param name="filter">Expression to filter the select query.</param>
        /// <returns></returns>
        List<T> LoadList(Expression<Func<T, bool>> filter = null);

        /// <summary>
        ///     DeleteItem item from the database
        /// </summary>
        /// <param name="itemToDelete">Item to delete.</param>
        void DeleteItem(T itemToDelete);
    }
}