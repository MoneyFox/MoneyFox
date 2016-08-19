using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Resources;
using PropertyChanged;

namespace MoneyFox.Shared.Repositories
{
    [ImplementPropertyChanged]
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDataAccess<Category> dataAccess;

        private List<Category> data;

        /// <summary>
        ///     Creates a CategoryRepository Object
        /// </summary>
        /// <param name="dataAccess">Instanced Category data Access</param>
        public CategoryRepository(IDataAccess<Category> dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public IEnumerable<Category> GetList(Expression<Func<Category, bool>> filter = null)
        {
            if (data == null)
            {
                data = dataAccess.LoadList();
            }

            return filter != null ? data.Where(filter.Compile()) : data;
        }

        public Category FindById(int id)
        {
            if (data == null)
            {
                data = dataAccess.LoadList();
            }

            return data.FirstOrDefault(c => c.Id == id);
        }

        /// <summary>
        ///     Save a new category or update an existing one.
        /// </summary>
        /// <param name="category">accountToDelete to save</param>
        /// <returns>Whether the task has succeeded</returns>
        public bool Save(Category category)
        {
            if (data == null)
            {
                data = dataAccess.LoadList();
            }

            if (string.IsNullOrWhiteSpace(category.Name))
            {
                category.Name = Strings.NoNamePlaceholderLabel;
            }

            if (category.Id == 0)
            {
                data.Add(category);
                data = new List<Category>(data.OrderBy(x => x.Name));
            }
            return dataAccess.SaveItem(category);
        }


        /// <summary>
        ///     Deletes the passed category and removes it from cache
        /// </summary>
        /// <param name="categoryToDelete">accountToDelete to delete</param>
        /// <returns>Whether the task has succeeded</returns>
        public bool Delete(Category categoryToDelete)
        {
            if (data == null)
            {
                data = dataAccess.LoadList();
            }

            data.Remove(categoryToDelete);
            return dataAccess.DeleteItem(categoryToDelete);
        }

        /// <summary>
        ///     Loads all categories from the database to the data collection
        /// </summary>
        public void Load(Expression<Func<Category, bool>> filter = null)
        {
            if (data == null)
            {
                data = dataAccess.LoadList();
            }
        }
    }
}