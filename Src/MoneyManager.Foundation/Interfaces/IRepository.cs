using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace MoneyManager.Foundation.Interfaces
{
    public interface IRepository<T>
    {
        /// <summary>
        ///     All item loaded from the database
        /// </summary>
        ObservableCollection<T> Data { get; set; }

        /// <summary>
        ///     The selected Item
        /// </summary>
        T Selected { get; set; }

        /// <summary>
        ///     Will update an existing entry and add a non existing
        /// </summary>
        /// <param name="item">Item.</param>
        void Save(T item);

        /// <summary>
        ///     DeleteItem the specified item.
        /// </summary>
        /// <param name="item">Item.</param>
        void Delete(T item);

        /// <summary>
        ///     Loads the data from the database and fills it to the data collection.
        /// </summary>
        void Load(Expression<Func<T, bool>> filter = null);
    }
}