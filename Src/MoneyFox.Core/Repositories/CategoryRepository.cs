using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using MoneyFox.Core.Model;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Model;
using MoneyFox.Foundation.Resources;
using MoneyManager.Foundation.Interfaces;
using PropertyChanged;

namespace MoneyFox.Core.Repositories
{
    [ImplementPropertyChanged]
    public class CategoryRepository : IRepository<Category>
    {
        private readonly IGenericDataRepository<Category> dataAccess;
        private ObservableCollection<Category> data;

        /// <summary>
        ///     Creates a CategoryRepository Object
        /// </summary>
        /// <param name="dataAccess">Instanced a <see cref="IGenericDataRepository{T}"/> type of <see cref="Category"/></param>
        public CategoryRepository(IGenericDataRepository<Category> dataAccess)
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
                dataAccess.Update(category);
            }
            dataAccess.Add(category);
        }

        /// <summary>
        ///     Deletes the passed category and removes it from cache
        /// </summary>
        /// <param name="categoryToDelete">accountToDelete to delete</param>
        public void Delete(Category categoryToDelete)
        {
            data.Remove(categoryToDelete);
            dataAccess.Delete(categoryToDelete);
        }

        /// <summary>
        ///     Loads all categories from the database to the data collection
        /// </summary>
        public void Load(Expression<Func<Category, bool>> filter = null)
        {
            Data.Clear();
            foreach (var category in dataAccess.GetList(filter))
            {
                Data.Add(category);
            }
        }
    }
}