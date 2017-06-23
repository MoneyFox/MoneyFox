using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MoneyFox.DataAccess.Infrastructure
{
    /// <summary>
    ///     Provides base operations who are used on all repositories
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RepositoryBase<T> where T : class
    {
        /// <summary>
        ///     Currenly used DbSet.
        /// </summary>
        protected readonly DbSet<T> DbSet;

        /// <summary>
        ///     Default Constructor
        /// </summary>
        /// <param name="dbFactory">Datacontext to work with.</param>
        protected RepositoryBase(IDbFactory dbFactory)
        {
            DbContext = dbFactory.Init().Result;
            DbSet = DbContext.Set<T>();
        }

        /// <summary>
        ///     Current ApplicationContex.
        /// </summary>
        protected ApplicationContext DbContext { get; }

        #region Implementation

        /// <summary>
        ///     Creates a new entry for the entity.
        /// </summary>
        /// <param name="entity">Entity to create.</param>
        public virtual void Add(T entity)
        {
            DbSet.Add(entity);
        }

        /// <summary>
        ///     Updats an existing entity
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        public virtual void Update(T entity)
        {
            DbSet.Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        ///     Deletes an existing entity.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        public virtual void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        /// <summary>
        ///     Deletes an existing entity who maches the passed filter.
        /// </summary>
        /// <param name="where">Filter for items to delete.</param>
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            var objects = DbSet.Where(where).AsEnumerable();
            foreach (var obj in objects)
                DbSet.Remove(obj);
        }

        /// <summary>
        ///     Get an item by the id. Returns Null if no item is found.
        /// </summary>
        /// <param name="id">Id of the item.</param>
        /// <returns>selected Item.</returns>
        public virtual async Task<T> GetById(int id)
        {
            return await DbSet.FindAsync(id);
        }

        /// <summary>
        ///     Get the first item who matches the passed filter.
        ///     Returns null if no item matches.
        /// </summary>
        /// <param name="where">Filter to seleect for.</param>
        /// <returns>First item that matched.</returns>
        public async Task<T> Get(Expression<Func<T, bool>> where)
        {
            return await DbSet.Where(where).FirstOrDefaultAsync();
        }

        /// <summary>
        ///     Returns all Items of the database table
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> GetAll()
        {
            return DbSet;
        }

        /// <summary>
        ///     Returns all Items who match the passed filter.
        /// </summary>
        /// <param name="where">Filter to select for.</param>
        /// <returns>Selected Items.</returns>
        public virtual IQueryable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return DbSet.Where(where);
        }

        #endregion
    }
}