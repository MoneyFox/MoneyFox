#region

using System.Collections.Generic;
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

        protected override List<Category> GetListFromDb() {
            using (var dbConn = SqlConnectionFactory.GetSqlConnection()) {
                return dbConn.Table<Category>()
                    .OrderBy(x => x.Name)
                    .ToList();
            }
        }
    }
}