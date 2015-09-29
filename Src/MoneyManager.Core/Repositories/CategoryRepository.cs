using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
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
            data = new ObservableCollection<Category>(this.dataAccess.LoadList());
        }

        /// <summary>
        ///     Cached category data
        /// </summary>
        public ObservableCollection<Category> Data
        {
            get { return data ?? (data = new ObservableCollection<Category>(dataAccess.LoadList())); }
            set
            {
                if (data == null)
                {
                    data = new ObservableCollection<Category>(dataAccess.LoadList());
                }
                if (Equals(data, value))
                {
                    return;
                }
                data = value;
            }
        }

        public Category Selected { get; set; }

        /// <summary>
        ///     SaveItem a new item or update an existin one.
        /// </summary>
        /// <param name="item">item to save</param>
        public void Save(Category item)
        {
            if (string.IsNullOrWhiteSpace(item.Name))
            {
                item.Name = Strings.NoNamePlaceholderLabel;
            }

            if (item.Id == 0)
            {
                data.Add(item);
            }
            dataAccess.SaveItem(item);
        }

        /// <summary>
        ///     Deletes the passed item and removes the item from cache
        /// </summary>
        /// <param name="item">item to delete</param>
        public void Delete(Category item)
        {
            data.Remove(item);
            dataAccess.DeleteItem(item);
        }

        /// <summary>
        ///     Loads all categories from the database to the data collection
        /// </summary>
        public void Load(Expression<Func<Category, bool>> filter = null)
        {
            Data = new ObservableCollection<Category>(dataAccess.LoadList(filter));
        }
    }
}