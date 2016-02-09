using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MvvmCross.Plugins.Sqlite.WindowsUWP;

namespace MoneyManager.Windows.DataAccess.Tests
{
    [TestClass]
    public class CategoryDataAccessTests
    {
        private SqliteConnectionCreator connectionCreator;

        [TestInitialize]
        public void Init()
        {
            connectionCreator = new SqliteConnectionCreator(new WindowsSqliteConnectionFactory());
        }

        [TestCleanup]
        public void Cleanup()
        {
            var dataAccess = new CategoryDataAccess(connectionCreator);
            var list = dataAccess.LoadList();

            foreach (var category in list)
            {
                dataAccess.DeleteItem(category);
            }
        }

        [TestMethod]
        public void SaveToDatabase_NewCategory_CorrectId()
        {
            var name = "TestCategory";

            var category = new Category
            {
                Name = name
            };

            new CategoryDataAccess(connectionCreator).SaveItem(category);

            Assert.AreEqual(1, category.Id);
            Assert.AreEqual(name, category.Name);
        }

        [TestMethod]
        public void SaveToDatabase_ExistingCategory_CorrectId()
        {
            var category = new Category();

            var dataAccess = new CategoryDataAccess(connectionCreator);
            dataAccess.SaveItem(category);

            Assert.IsNull(category.Name);

            var id = category.Id;

            var name = "TestCategory";
            category.Name = name;

            Assert.AreEqual(id, category.Id);
            Assert.AreEqual(name, category.Name);
        }

        [TestMethod]
        public void SaveToDatabase_MultipleCategories_AllSaved()
        {
            var category1 = new Category
            {
                Name = "Einkaufen",
            };

            var category2 = new Category
            {
                Name = "Beer",
            };

            var dataAccess = new CategoryDataAccess(connectionCreator);
            dataAccess.SaveItem(category1);
            dataAccess.SaveItem(category2);

            var resultList = dataAccess.LoadList();

            Assert.AreEqual(2, resultList.Count);
        }
    }
}