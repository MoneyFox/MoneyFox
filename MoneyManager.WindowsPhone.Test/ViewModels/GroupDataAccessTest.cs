using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace MoneyManager.WindowsPhone.Test.ViewModels
{
    [TestClass]
    internal class GroupDataAccessTest
    {
        private Group group;

        private GroupDataAccess groupviewModel
        {
            get { return ServiceLocator.Current.GetInstance<GroupDataAccess>(); }
        }

        [TestInitialize]
        public async Task InitTests()
        {
            await DatabaseHelper.CreateDatabase();

            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                dbConn.DeleteAll<Group>();
                if (groupviewModel.AllGroups != null)
                {
                    groupviewModel.AllGroups.Clear();
                }
            }

            group = new Group
            {
                Name = "Sparkonten"
            };
        }

        [TestMethod]
        public void SaveGroupTest()
        {
            groupviewModel.Save(group);

            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                var saved = dbConn.Table<Group>().Where(x => x.Name == group.Name).ToList().First();
                Assert.IsTrue(saved.Name == group.Name);
            }
        }

        [TestMethod]
        public void LoadGroupListTest()
        {
            groupviewModel.Save(group);
            groupviewModel.Save(group);
            Assert.AreEqual(groupviewModel.AllGroups.Count, 2);

            groupviewModel.AllGroups = null;
            groupviewModel.LoadList();
            Assert.AreEqual(groupviewModel.AllGroups.Count, 2);
        }

        [TestMethod]
        public void UpateGroupTest()
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                groupviewModel.Save(group);
                Assert.AreEqual(groupviewModel.AllGroups.Count, 1);

                string newName = "This is a new Name";

                group = dbConn.Table<Group>().First();
                group.Name = newName;
                groupviewModel.Update(group);

                Assert.AreEqual(newName, dbConn.Table<Group>().First().Name);
            }
        }

        [TestMethod]
        public void DeleteGroupTest()
        {
            groupviewModel.Save(group);
            Assert.IsTrue(groupviewModel.AllGroups.Contains(group));

            groupviewModel.Delete(group, true);
            Assert.IsFalse(groupviewModel.AllGroups.Contains(group));
        }
    }
}