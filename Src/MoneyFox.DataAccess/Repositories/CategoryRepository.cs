using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using MoneyFox.DataAccess.DatabaseModels;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Foundation.Resources;

namespace MoneyFox.DataAccess.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDatabaseManager dbManager;

        /// <summary>
        ///     Creates a CategoryRepository Object
        /// </summary>
        /// <param name="dbManager">Instanced <see cref="IDatabaseManager"/> data Access</param>
        public CategoryRepository(IDatabaseManager dbManager)
        {
            this.dbManager = dbManager;
        }

        public IEnumerable<CategoryViewModel> GetList(Expression<Func<CategoryViewModel, bool>> filter = null)
        {
            using (var db = dbManager.GetConnection())
            {
				var query = db.Table<Category>().AsQueryable();
				var newFilter = Mapper.Map<Expression<Func<Category, bool>>>(filter);

                if (filter != null)
                {
                    query = query.Where(newFilter);
                }
				return Mapper.Map<List<CategoryViewModel>>(query.ToList());
            }
        }

        public CategoryViewModel FindById(int id)
        {
            using (var db = dbManager.GetConnection())
            {
                return Mapper.Map<CategoryViewModel>(db.Table<Category>().FirstOrDefault(x => x.Id == id));
            }
        }

        /// <summary>
        ///     Save a new CategoryViewModel or update an existing one.
        /// </summary>
        public bool Save(CategoryViewModel categoryVmToSave)
        {
            using (var db = dbManager.GetConnection())
            {
                if (string.IsNullOrWhiteSpace(categoryVmToSave.Name))
                {
                    categoryVmToSave.Name = Strings.NoNamePlaceholderLabel;
                }

                var category = Mapper.Map<Category>(categoryVmToSave);

                if (category.Id == 0)
                {
                    var rows = db.Insert(category);
                    categoryVmToSave.Id = db.Table<Category>().OrderByDescending(x => x.Id).First().Id;
                    return rows == 1;
                }
                return db.Update(category) == 1;
            }
        }

        /// <summary>
        ///     Deletes the passed CategoryViewModel and removes it from cache
        /// </summary>
        public bool Delete(CategoryViewModel accountToDelete)
        {
            using (var db = dbManager.GetConnection())
            {
                var itemToDelete = db.Table<Category>().Single(x => x.Id == accountToDelete.Id);
                return db.Delete(itemToDelete) == 1;
            }
        }
    }
}