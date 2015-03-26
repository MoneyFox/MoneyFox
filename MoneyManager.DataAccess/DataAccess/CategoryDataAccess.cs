#region

using System.Collections.ObjectModel;
using System.Linq;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using PropertyChanged;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;

#endregion

namespace MoneyManager.DataAccess.DataAccess {
    [ImplementPropertyChanged]
    public class CategoryDataAccess : AbstractDataAccess<Category> {
        public ObservableCollection<Category> AllCategories { get; set; }

        public Category SelectedCategory { get; set; }

        protected override void SaveToDb(Category itemToSave) {
            using (var db = SqlConnectionFactory.GetSqlConnection()) {
                if (itemToSave.Id == 0) {
                    db.InsertWithChildren(itemToSave);
                } else {
                    db.UpdateWithChildren(itemToSave);
                }
            }
        }

        protected override void DeleteFromDatabase(Category category) {
            using (var dbConn = SqlConnectionFactory.GetSqlConnection()) {
                if (AllCategories != null) {
                    AllCategories.Remove(category);
                }
                dbConn.Delete(category);
            }
        }

        protected override void GetListFromDb() {
            using (var dbConn = SqlConnectionFactory.GetSqlConnection()) {
                AllCategories = new ObservableCollection<Category>(dbConn.Table<Category>()
                    .ToList()
                    .OrderBy(x => x.Name));
            }
        }
    }
}