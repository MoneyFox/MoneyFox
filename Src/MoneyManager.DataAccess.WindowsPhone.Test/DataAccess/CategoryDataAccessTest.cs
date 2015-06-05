#region

using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.Foundation.Model;

#endregion

namespace MoneyManager.DataAccess.WindowsPhone.Test.DataAccess
{
    [TestClass]
    public class CategoryDataAccessTest
    {
        [TestInitialize]
        public void TestInit()
        {
            using (var db = SqlConnectionFactory.GetSqlConnection())
            {
                db.CreateTable<Category>();
            }
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void CategoryDataAccess_CrudCategory()
        {
            var categoryDataAccess = new CategoryDataAccess();

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