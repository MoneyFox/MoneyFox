using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Core;
using MoneyManager.Core.DataAccess;
using MoneyManager.Foundation.Model;
using MoneyManager.Windows.Core.Tests.Helper;
using SQLite.Net.Platform.WinRT;

namespace MoneyManager.Windows.Core.Tests.DataAccess
{
    [TestClass]
    public class CategoryDataAccessTest
    {
        [TestMethod]
        [TestCategory("Integration")]
        public void CategoryDataAccess_CrudCategory()
        {
            var categoryDataAccess =
                new CategoryDataAccess(new DbHelper(new SQLitePlatformWinRT(), new TestDatabasePath()));

            const string firstName = "category";
            const string secondName = "new category";

            var category = new Category
            {
                Name = firstName
            };

            categoryDataAccess.Save(category);

            categoryDataAccess.LoadList();
            var list = categoryDataAccess.LoadList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(firstName, list.First().Name);

            category.Name = secondName;
            categoryDataAccess.Save(category);

            list = categoryDataAccess.LoadList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(secondName, list.First().Name);

            categoryDataAccess.Delete(category);

            list = categoryDataAccess.LoadList();
            Assert.IsFalse(list.Any());
        }
    }
}