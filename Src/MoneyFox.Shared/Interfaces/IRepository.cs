using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace MoneyFox.Shared.Interfaces {
    public interface IRepository<T> {
        /// <summary>
        ///     All payment loaded from the database
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
        ///     Delete the specified payment.
        /// </summary>
        /// <param name="paymentToDelete">Payment to delete.</param>
        void Delete(T paymentToDelete);

        /// <summary>
        ///     Loads the data from the database and fills it to the data collection.
        /// </summary>
        void Load(Expression<Func<T, bool>> filter = null);
    }
}