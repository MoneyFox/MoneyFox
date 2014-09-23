using MoneyManager.Models;
using MoneyManager.Src;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Linq;

namespace MoneyManager.DataAccess
{
    [ImplementPropertyChanged]
    public class CategoryDataAccess : AbstractDataAccess<Category>
    {
        public ObservableCollection<Category> AllCategories { get; set; }

        public Category SelectedCategory { get; set; }

        public CategoryDataAccess()
        {
            LoadList();
        }

        protected override void SaveToDb(Category category)
        {
            if (AllCategories == null)
            {
                AllCategories = new ObservableCollection<Category>();
            }

            category.Id = Utilities.GetMaxId();

            AllCategories.Add(category);

            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            roamingSettings.Values[category.Id.ToString()] = category.Name;
        }

        protected override void DeleteFromDatabase(Category category)
        {
            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            roamingSettings.Values.Remove(category.Id.ToString());

            AllCategories.Remove(category);
        }

        protected override void GetListFromDb()
        {
            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

            AllCategories = new ObservableCollection<Category>(roamingSettings.Values
                .Select(x => new Category
                {
                    Id = int.Parse(x.Key),
                    Name = x.Value.ToString()
                }).ToList());
        }

        protected override void UpdateItem(Category category)
        {
            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            roamingSettings.Values[category.Id.ToString()] = category.Name;
        }

        public void DeleteAll()
        {
            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            roamingSettings.Values.Clear();
        }
    }
}