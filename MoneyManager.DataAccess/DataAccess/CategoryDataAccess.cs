using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;
using PropertyChanged;

namespace MoneyManager.DataAccess.DataAccess
{
    [ImplementPropertyChanged]
    internal class CategoryDataAccess : AbstractDataAccess<Category>
    {    
        protected override void SaveToDb(Category category)
        {
            using (var dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                if (AllCategories == null)
                {
                    LoadList();
                }

                AllCategories.Add(category);
                category.Id = dbConn.Insert(category);
            }
        }

        protected override void DeleteFromDatabase(Category categorycategory)
        {
            using (var dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                AllCategories.Remove(categorycategory);
                dbConn.Delete(category);
            }
        }

        protected override void GetListFromDb()
        {
            using (var dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                AllCategories = new ObservableCollection<Category>(dbConn.Table<Category>().ToList());
            }
        }

        protected override void UpdateItem(Category category)
        {
            using (var dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                dbConn.Update(category, typeof (Category));
            }
        }
    }
}
