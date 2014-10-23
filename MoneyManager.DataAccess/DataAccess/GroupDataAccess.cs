using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;
using PropertyChanged;
using SQLite.Net;

namespace MoneyManager.DataAccess.DataAccess
{
    [ImplementPropertyChanged]
    internal class GroupDataAccess : AbstractDataAccess<Group>
    {
        public ObservableCollection<Group> AllGroups { get; set; }

        protected override void SaveToDb(Group group)
        {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                if (AllGroups == null)
                {
                    AllGroups = new ObservableCollection<Group>();
                }

                AllGroups.Add(group);
                group.Id = dbConn.Insert(group);
            }
        }

        protected override void DeleteFromDatabase(Group group)
        {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                AllGroups.Remove(group);
                dbConn.Delete(group);
            }
        }

        protected override List<Group> GetListFromDb()
        {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                return dbConn.Table<Group>().ToList();
            }
        }

        protected override void UpdateItem(Group group)
        {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                dbConn.Update(group, typeof (Group));
            }
        }
    }
}