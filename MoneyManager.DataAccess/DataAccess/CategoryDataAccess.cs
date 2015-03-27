using System.Collections.Generic;
using System.Linq;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using PropertyChanged;
using SQLiteNetExtensions.Extensions;

namespace MoneyManager.DataAccess.DataAccess {
    [ImplementPropertyChanged]
    public class CategoryDataAccess : AbstractDataAccess<Category> {
        protected override void SaveToDb(Category itemToSave) {
            using (var db = SqlConnectionFactory.GetSqlConnection()) {
                if (itemToSave.Id == 0) {
                    db.InsertWithChildren(itemToSave);
                }
                else {
                    db.UpdateWithChildren(itemToSave);
                }
            }
        }

        protected override void DeleteFromDatabase(Category category) {
            using (var dbConn = SqlConnectionFactory.GetSqlConnection()) {
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