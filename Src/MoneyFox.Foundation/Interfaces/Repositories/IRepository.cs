using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MoneyFox.Foundation.Interfaces.Repositories
{
    public interface IRepository<T>
    {
        /// <summary>
        ///     Returns a List filtered by the passed expression.
        /// </summary>
        /// <param name="filter">filter expression.</param>
        /// <returns>Ienumerable with the selected items.</returns>
        IEnumerable<T> GetList(Expression<Func<T, bool>> filter = null);

        /// <summary>
        ///     Finds an entity by its id
        /// </summary>
        /// <param name="id">Id of entity to find.</param>
        T FindById(int id);

        /// <summary>
        ///     Will update an existing entry and add a non existing
        /// </summary>
        /// <param name="item">Item.</param>
        bool Save(T item);

        /// <summary>
        ///     Delete the specified payment.
        /// </summary>
        /// <param name="paymentToDelete">PaymentViewModel to delete.</param>
        bool Delete(T paymentToDelete);

        /// <summary>
        ///     Loads the data from the database and fills it to the data collection.
        /// </summary>
        void Load(Expression<Func<T, bool>> filter = null);
    }
}