using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MoneyFox.DataAccess.Repositories
{
    public interface IRepository<T>
    {
        /// <summary>
        ///     Creates a new entry for the entity.
        /// </summary>
        /// <param name="entity">Entity to create.</param>
        void Add(T entity);

        /// <summary>
        ///     Updats an existing entity
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        void Update(T entity);

        /// <summary>
        ///     Deletes an existing entity.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        void Delete(T entity);

        /// <summary>
        ///     Deletes an existing entity who maches the passed filter.
        /// </summary>
        /// <param name="where">Filter for items to delete.</param>
        void Delete(Expression<Func<T, bool>> where);

        /// <summary>
        ///     Returns all Items of the database table
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAll();

        /// <summary>
        ///     Returns a queryable of all Items who match the passed filter.
        /// </summary>
        /// <param name="where">Filter to select for.</param>
        /// <returns>Selected Items.</returns>
        IQueryable GetMany(Expression<Func<T, bool>> where);

        /// <summary>
        ///     Get the first item who matches the passed filter.
        ///     Returns null if no item matches.
        /// </summary>
        /// <param name="where">Filter to seleect for.</param>
        /// <returns>First item that matched.</returns>
        Task<T> Get(Expression<Func<T, bool>> where);

        /// <summary>
        ///     Get an item by the id. Returns Null if no item is found.
        /// </summary>
        /// <param name="id">Id of the item.</param>
        /// <returns>selected Item.</returns>
        Task<T> GetById(int id);
    }
}
