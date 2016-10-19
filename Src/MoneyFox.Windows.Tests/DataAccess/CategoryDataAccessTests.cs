using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyFox.Shared;
using MoneyFox.Shared.DataAccess;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MvvmCross.Plugins.File.WindowsCommon;
using MvvmCross.Plugins.Sqlite.WindowsUWP;

namespace MoneyFox.Windows.Tests.DataAccess
{
    [TestClass]
    public class CategoryDataAccessTests
    {
        private IDatabaseManager dbManager;

        [TestInitialize]
        public void Init()
        {
            dbManager = new DatabaseManager(new WindowsSqliteConnectionFactory(), new MvxWindowsCommonFileStore());
        }

        [TestMethod]
        public void SaveToDatabase_NewCategory_CorrectId()
        {
            var name = "TestCategory";

            var category = new CategoryViewModel
            {
                Name = name
            };

            new CategoryDataAccess(dbManager).SaveItem(category);

            Assert.IsTrue(category.Id >= 1);
            Assert.AreEqual(name, category.Name);
        }

        [TestMethod]
        public void SaveToDatabase_ExistingCategory_CorrectId()
        {
            var category = new CategoryViewModel();

            var dataAccess = new CategoryDataAccess(dbManager);
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
            var category1 = new CategoryViewModel
            {
                Name = "Einkaufen"
            };

            var category2 = new CategoryViewModel
            {
                Name = "Beer"
            };

            var dataAccess = new CategoryDataAccess(dbManager);
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

            var category = new CategoryViewModel
            {
                Name = firstName
            };

            var dataAccess = new CategoryDataAccess(dbManager);
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
            var category = new CategoryViewModel
            {
                Name = "categoryToDelete"
            };

            var dataAccess = new CategoryDataAccess(dbManager);
            dataAccess.SaveItem(category);

            Assert.IsTrue(dataAccess.LoadList(x => x.Id == category.Id).Any());

            dataAccess.DeleteItem(category);
            Assert.IsFalse(dataAccess.LoadList(x => x.Id == category.Id).Any());
        }
    }
}