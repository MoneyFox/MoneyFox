using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;
using SQLite.Net;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MoneyManager.DataAccess.DataAccess
{
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
            using (var dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                AllGroups.Remove(group);
                dbConn.Delete(group);
            }
        }

        protected override List<Group> GetListFromDb()
        {
            using (var dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                return dbConn.Table<Group>().ToList();
            }
        }

        protected override void UpdateItem(Group group)
        {
            using (var dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                dbConn.Update(group, typeof(Group));
            }
        }
    }
}