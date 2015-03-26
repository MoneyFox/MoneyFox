#region

using System.Collections.ObjectModel;
using System.Linq;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using PropertyChanged;
using SQLite.Net;

#endregion

namespace MoneyManager.DataAccess.DataAccess {
    [ImplementPropertyChanged]
    public class CategoryDataAccess : AbstractDataAccess<Category> {
        public ObservableCollection<Category> AllCategories { get; set; }

        public Category SelectedCategory { get; set; }

        protected override void SaveToDb(Category category) {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection()) {
                if (AllCategories == null) {
                    LoadList();
                }

                AllCategories.Add(category);
                AllCategories = new ObservableCollection<Category>(AllCategories.OrderBy(x => x.Name));
                dbConn.Insert(category);
            }
        }

        protected override void DeleteFromDatabase(Category category) {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection()) {
                if (AllCategories != null) {
                    AllCategories.Remove(category);
                }
                dbConn.Delete(category);
            }
        }

        protected override void GetListFromDb() {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection()) {
                AllCategories = new ObservableCollection<Category>(dbConn.Table<Category>()
                    .ToList()
                    .OrderBy(x => x.Name));
            }
        }

        protected override void UpdateItem(Category category) {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection()) {
                dbConn.Update(category, typeof (Category));
            }
        }
    }
}