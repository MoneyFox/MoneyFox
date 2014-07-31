using MoneyManager.DataAccess;
using MoneyTracker.Models;
using MoneyTracker.Src;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Linq;

namespace MoneyTracker.ViewModels
{
    [ImplementPropertyChanged]
    public class GroupDAO : AbstractDataAccess<Group>
    {
        public ObservableCollection<Group> AllGroups { get; set; }

        protected override void SaveToDb(Group group)
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
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
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                AllGroups.Remove(group);
                dbConn.Delete(group);
            }
        }

        protected override void GetListFromDb()
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                AllGroups = new ObservableCollection<Group>(dbConn.Table<Group>().ToList());
            }
        }

        protected override void UpdateItem(Group group)
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                dbConn.Update(group, typeof(Group));
            }
        }
    }
}