using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Resources;
using PropertyChanged;

namespace MoneyFox.Shared.Repositories
{
    [ImplementPropertyChanged]
    public class CategoryRepository : IRepository<Category>
    {
        private readonly IDataAccess<Category> dataAccess;
        private readonly INotificationService notificationService;

        private ObservableCollection<Category> data;

        /// <summary>
        ///     Creates a CategoryRepository Object
        /// </summary>
        /// <param name="dataAccess">Instanced Category data Access</param>
        public CategoryRepository(IDataAccess<Category> dataAccess, INotificationService notificationService)
        {
            this.dataAccess = dataAccess;
            this.notificationService = notificationService;

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
            if (dataAccess.SaveItem(category))
            {
                notificationService.SendBasicNotification(Strings.ErrorTitleSave, Strings.ErrorMessageSave);
            } else
            {
                Settings.LastDatabaseUpdate = DateTime.Now;
            }
        }

        /// <summary>
        ///     Deletes the passed category and removes it from cache
        /// </summary>
        /// <param name="categoryToDelete">accountToDelete to delete</param>
        public void Delete(Category categoryToDelete)
        {
            data.Remove(categoryToDelete);
            if (dataAccess.DeleteItem(categoryToDelete))
            {
                notificationService.SendBasicNotification(Strings.ErrorTitleDelete, Strings.ErrorMessageDelete);
            } else
            {
                Settings.LastDatabaseUpdate = DateTime.Now;
            }
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