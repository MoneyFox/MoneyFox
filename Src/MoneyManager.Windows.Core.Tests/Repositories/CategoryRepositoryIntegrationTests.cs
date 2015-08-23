using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Core;
using MoneyManager.Core.DataAccess;
using MoneyManager.Core.Repositories;
using MoneyManager.Foundation.Model;
using MoneyManager.Windows.Core.Tests.Helper;
using SQLite.Net.Platform.WinRT;
using SQLiteNetExtensions.Extensions;

namespace MoneyManager.Windows.Core.Tests.Repositories
{
    [TestClass]
    public class CategoryRepositoryIntegrationTests
    {
        [TestMethod]
        [TestCategory("Integration")]
        public void CategoryRepositor_LoadDataFromDbThroughRepository()
        {
            var dbHelper = new DbHelper(new SQLitePlatformWinRT(), new TestDatabasePath());

            using (var db = dbHelper.GetSqlConnection())
            {
                db.DeleteAll<Category>();
                db.InsertWithChildren(new Category
                {
                    Name = "Foooo"
                });
            }

            var repository = new CategoryRepository(new CategoryDataAccess(dbHelper));

            Assert.IsTrue(repository.Data.Any());
            Assert.AreEqual("Foooo", repository.Data[0].Name);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void CategoryRepository_Update()
        {
            var dbHelper = new DbHelper(new SQLitePlatformWinRT(), new TestDatabasePath());

            using (var db = dbHelper.GetSqlConnection())
            {
                db.DeleteAll<Category>();
            }

            var repository = new CategoryRepository(new CategoryDataAccess(dbHelper));

            var category = new Category
            {
                Name = "Ausgang"
            };

            repository.Save(category);
            Assert.AreEqual(1, repository.Data.Count);
            Assert.AreSame(category, repository.Data[0]);

            category.Name = "newName";

            repository.Save(category);

            Assert.AreEqual(1, repository.Data.Count);
            Assert.AreEqual("newName", repository.Data[0].Name);
        }
    }
}