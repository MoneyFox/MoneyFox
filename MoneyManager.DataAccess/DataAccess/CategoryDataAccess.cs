using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage;

namespace MoneyManager.DataAccess.DataAccess
{
    internal class CategoryDataAccess : AbstractDataAccess<Category>
    {
        public CategoryDataAccess()
        {
            LoadList();
        }

        public ObservableCollection<Category> AllCategories { get; set; }

        public Category SelectedCategory { get; set; }

        protected override void SaveToDb(Category category)
        {
            if (AllCategories == null)
            {
                AllCategories = new ObservableCollection<Category>();
            }

            //TODO: Refactor
            //category.Id = Utilities.GetMaxId();

            AllCategories.Add(category);
            AllCategories = new ObservableCollection<Category>(AllCategories.OrderBy(x => x.Name));

            ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
            roamingSettings.Values[category.Id.ToString()] = category.Name;
        }

        protected override void DeleteFromDatabase(Category category)
        {
            ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
            roamingSettings.Values.Remove(category.Id.ToString());

            AllCategories.Remove(category);
        }

        protected override List<Category> GetListFromDb()
        {
            ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;

            return roamingSettings.Values
                .OrderBy(x => x.Value)
                .Select(x => new Category
                {
                    Id = int.Parse(x.Key),
                    Name = x.Value.ToString()
                }).ToList();
        }

        protected override void UpdateItem(Category category)
        {
            ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
            roamingSettings.Values[category.Id.ToString()] = category.Name;
        }

        public void DeleteAll()
        {
            ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
            roamingSettings.Values.Clear();

            AllCategories.Clear();
        }
    }
}