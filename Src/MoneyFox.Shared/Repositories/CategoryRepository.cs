using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Resources;
using PropertyChanged;

namespace MoneyFox.Shared.Repositories {
    [ImplementPropertyChanged]
    public class CategoryRepository : ICategoryRepository {
        private readonly IDataAccess<Category> dataAccess;

        private ObservableCollection<Category> data;

        /// <summary>
        ///     Creates a CategoryRepository Object
        /// </summary>
        /// <param name="dataAccess">Instanced Category data Access</param>
        public CategoryRepository(IDataAccess<Category> dataAccess) {
            this.dataAccess = dataAccess;

            Data = new ObservableCollection<Category>();
            Load();
        }

        /// <summary>
        ///     Cached category data
        /// </summary>
        public ObservableCollection<Category> Data {
            get { return data; }
            set {
                if (Equals(data, value)) {
                    return;
                }
                data = value;
            }
        }

        public Category FindById(int id) => data.FirstOrDefault(c => c.Id == id);

        public Category Selected { get; set; }

        /// <summary>
        ///     Save a new category or update an existing one.
        /// </summary>
        /// <param name="category">accountToDelete to save</param>
        /// <returns>Whether the task has succeeded</returns>
        public bool Save(Category category) {
            if (string.IsNullOrWhiteSpace(category.Name)) {
                category.Name = Strings.NoNamePlaceholderLabel;
            }

            if (category.Id == 0) {
                data.Add(category);

                data = new ObservableCollection<Category>(data.OrderBy(x => x.Name));
            }
            return dataAccess.SaveItem(category);
        }


        /// <summary>
        ///     Deletes the passed category and removes it from cache
        /// </summary>
        /// <param name="categoryToDelete">accountToDelete to delete</param>
        /// <returns>Whether the task has succeeded</returns>
        public bool Delete(Category categoryToDelete) {
            data.Remove(categoryToDelete);
            return dataAccess.DeleteItem(categoryToDelete);
        }

        /// <summary>
        ///     Loads all categories from the database to the data collection
        /// </summary>
        public void Load(Expression<Func<Category, bool>> filter = null) {
            Data.Clear();

            foreach (var category in dataAccess.LoadList(filter)) {
                Data.Add(category);
            }
        }
    }
}