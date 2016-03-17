using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

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

        [TestMethod]
        public void SaveToDatabase_NewCategory_CorrectId()
        {
            var name = "TestCategory";

            var category = new Category
            {
                Name = name
            };

            new CategoryDataAccess(connectionCreator).SaveItem(category);

            Assert.IsTrue(category.Id >= 1);
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
                Name = "Einkaufen"
            };

            var category2 = new Category
            {
                Name = "Beer"
            };

            var dataAccess = new CategoryDataAccess(connectionCreator);
            dataAccess.SaveItem(category1);
            dataAccess.SaveItem(category2);

            var resultList = dataAccess.LoadList();

            Assert.IsTrue(resultList.Any(x => x.Id == category1.Id && x.Name == category1.Name));
            Assert.IsTrue(resultList.Any(x => x.Id == category2.Id && x.Name == category2.Name));
        }

        [TestMethod]
        public void SaveToDatabase_CreateAndUpdateCategory_CorrectlyUpdated()
        {
            var firstName = "old name";
            var secondName = "new name";

            var category = new Category
            {
                Name = firstName
            };

            var dataAccess = new CategoryDataAccess(connectionCreator);
            dataAccess.SaveItem(category);

            Assert.AreEqual(firstName, dataAccess.LoadList().FirstOrDefault(x => x.Id == category.Id).Name);

            category.Name = secondName;
            dataAccess.SaveItem(category);

            var categories = dataAccess.LoadList();
            Assert.IsFalse(categories.Any(x => x.Name == firstName));
            Assert.AreEqual(secondName, categories.First(x => x.Id == category.Id).Name);
        }

        [TestMethod]
        public void DeleteFromDatabase_CategoryToDelete_CorrectlyDelete()
        {
            var category = new Category
            {
                Name = "categoryToDelete"
            };

            var dataAccess = new CategoryDataAccess(connectionCreator);
            dataAccess.SaveItem(category);

            Assert.IsTrue(dataAccess.LoadList(x => x.Id == category.Id).Any());

            dataAccess.DeleteItem(category);
            Assert.IsFalse(dataAccess.LoadList(x => x.Id == category.Id).Any());
        }
    }
}