using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using MoneyManager.Core.Helpers;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MoneyManager.Localization;
using PropertyChanged;

namespace MoneyManager.Core.Repositories
{
    [ImplementPropertyChanged]
    public class CategoryRepository : IRepository<Category>
    {
        private readonly IDataAccess<Category> dataAccess;
        private ObservableCollection<Category> data;

        /// <summary>
        ///     Creates a CategoryRepository Object
        /// </summary>
        /// <param name="dataAccess">Instanced Category data Access</param>
        public CategoryRepository(IDataAccess<Category> dataAccess)
        {
            this.dataAccess = dataAccess;

            Data = new ObservableCollection<Category>();
            Load();
        }

        /// <summary>
        ///     Cached category data
        /// </summary>
        public ObservableCollection<Category> Data
        {
            get { return data; }
            set
            {
                if (Equals(data, value))
                {
                    return;
                }
                data = value;
            }
        }

        public Category Selected { get; set; }

        /// <summary>
        ///     Save a new category or update an existing one.
        /// </summary>
        /// <param name="category">accountToDelete to save</param>
        public void Save(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                category.Name = Strings.NoNamePlaceholderLabel;
            }

            if (category.Id == 0)
            {
                data.Add(category);
            }
            dataAccess.SaveItem(category);
            Settings.LastDatabaseUpdate = DateTime.Now;
        }

        /// <summary>
        ///     Deletes the passed category and removes it from cache
        /// </summary>
        /// <param name="categoryToDelete">accountToDelete to delete</param>
        public void Delete(Category categoryToDelete)
        {
            data.Remove(categoryToDelete);
            dataAccess.DeleteItem(categoryToDelete);
            Settings.LastDatabaseUpdate = DateTime.Now;
        }

        /// <summary>
        ///     Loads all categories from the database to the data collection
        /// </summary>
        public void Load(Expression<Func<Category, bool>> filter = null)
        {
            Data.Clear();
            foreach (var category in dataAccess.LoadList(filter))
            {
                Data.Add(category);
            }
        }
    }
}