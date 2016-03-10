using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MoneyFox.Foundation.Interfaces
{
    public interface IGenericDataRepository<T> where T : class
    {
        /// <summary>
        ///     Loads a list with all items from the database
        /// </summary>
        /// <param name="navigationProperties">Associations to load.</param>
        /// <returns>List with loaded items.</returns>
        IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties);

        /// <summary>
        ///     Loads a list of items from the database filtered with the filter expression.
        /// </summary>
        /// <param name="filter">Filter Expression.</param>
        /// <param name="navigationProperties">Associations to load.</param>
        /// <returns>List with loaded items.</returns>
        IList<T> GetList(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] navigationProperties);

        /// <summary>
        ///     Loads a single item from the database.
        /// </summary>
        /// <param name="filter">filter to for the item to load</param>
        /// <param name="navigationProperties">Associations to load.</param>
        /// <returns>Loaded item.</returns>
        T GetSingle(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] navigationProperties);

        /// <summary>
        ///     Adds a new item to the database
        /// </summary>
        /// <param name="items">Item to save.</param>
        void Add(params T[] items);

        /// <summary>
        ///     Updates an existing item on the database
        /// </summary>
        /// <param name="items">Item to update.</param>
        void Update(params T[] items);

        /// <summary>
        ///     Removes an item from the database.
        /// </summary>
        /// <param name="items">Item to remove.</param>
        void Delete(params T[] items);
    }
}