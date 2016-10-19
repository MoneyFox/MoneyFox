using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Shared.Resources;

namespace MoneyFox.Shared.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDataAccess<CategoryViewModel> dataAccess;

        private List<CategoryViewModel> data;

        /// <summary>
        ///     Creates a CategoryRepository Object
        /// </summary>
        /// <param name="dataAccess">Instanced CategoryViewModel data Access</param>
        public CategoryRepository(IDataAccess<CategoryViewModel> dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public IEnumerable<CategoryViewModel> GetList(Expression<Func<CategoryViewModel, bool>> filter = null)
        {
            if (data == null)
            {
                data = dataAccess.LoadList();
            }

            return filter != null ? data.Where(filter.Compile()) : data;
        }

        public CategoryViewModel FindById(int id)
        {
            if (data == null)
            {
                data = dataAccess.LoadList();
            }

            return data.FirstOrDefault(c => c.Id == id);
        }

        /// <summary>
        ///     Save a new CategoryViewModel or update an existing one.
        /// </summary>
        /// <param name="category">accountToDelete to save</param>
        /// <returns>Whether the task has succeeded</returns>
        public bool Save(CategoryViewModel category)
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
                data = new List<CategoryViewModel>(data.OrderBy(x => x.Name));
            }
            return dataAccess.SaveItem(category);
        }


        /// <summary>
        ///     Deletes the passed CategoryViewModel and removes it from cache
        /// </summary>
        /// <param name="categoryToDelete">accountToDelete to delete</param>
        /// <returns>Whether the task has succeeded</returns>
        public bool Delete(CategoryViewModel categoryToDelete)
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
        public void Load(Expression<Func<CategoryViewModel, bool>> filter = null)
        {
            if (data == null)
            {
                data = dataAccess.LoadList();
            }
        }
    }
}