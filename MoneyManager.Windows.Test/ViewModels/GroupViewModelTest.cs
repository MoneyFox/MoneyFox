using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Models;
using MoneyManager.Src;
using MoneyManager.ViewModels.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Windows.Test.ViewModels
{
    [TestClass]
    public class GroupViewModelTest
    {
        private Group group;

        [TestInitialize]
        public async Task InitTests()
        {
            App.GroupViewModel = new GroupViewModel();
            await DatabaseHelper.CreateDatabase();

            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                dbConn.DeleteAll<Group>();
            }

            group = new Group
            {
                Name = "Sparkonten"
            };
        }

        [TestMethod]
        public void SaveGroupTest()
        {
            App.GroupViewModel.Save(group);

            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                var saved = dbConn.Table<Group>().Where(x => x.Name == group.Name).ToList().First();
                Assert.IsTrue(saved.Name == group.Name);
            }
        }

        [TestMethod]
        public void LoadGroupListTest()
        {
            App.GroupViewModel.Save(group);
            App.GroupViewModel.Save(group);
            Assert.AreEqual(App.GroupViewModel.AllGroups.Count, 2);

            App.GroupViewModel.AllGroups = null;
            App.GroupViewModel.LoadList();
            Assert.AreEqual(App.GroupViewModel.AllGroups.Count, 2);
        }

        [TestMethod]
        public void UpateGroupTest()
        {
            using (var dbConn = ConnectionFactory.GetDbConnection())
            {
                App.GroupViewModel.Save(group);
                Assert.AreEqual(App.GroupViewModel.AllGroups.Count, 1);

                string newName = "This is a new Name";

                group = dbConn.Table<Group>().First();
                group.Name = newName;
                App.GroupViewModel.Update(group);

                Assert.AreEqual(newName, dbConn.Table<Group>().First().Name);
            }
        }

        [TestMethod]
        public void DeleteGroupTest()
        {
            App.GroupViewModel.Save(group);
            Assert.IsTrue(App.GroupViewModel.AllGroups.Contains(group));

            App.GroupViewModel.Delete(group);
            Assert.IsFalse(App.GroupViewModel.AllGroups.Contains(group));
        }
    }
}