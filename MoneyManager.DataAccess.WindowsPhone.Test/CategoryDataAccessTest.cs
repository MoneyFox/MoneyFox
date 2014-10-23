using System;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;

namespace MoneyManager.DataAccess.WindowsPhone.Test
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
        public void CrudCategoryTest()
        {
            var categoryDataAccess = new CategoryDataAccess();

            const string firstName = "category";
            const string secondName = "new category";

            var category = new Category
            {
                Name = firstName
            };

            categoryDataAccess.Save(category);

            var list = categoryDataAccess.LoadList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(firstName, list.First().Name);

            category.Name = secondName;
            categoryDataAccess.Update(category);

            list = categoryDataAccess.LoadList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(secondName, list.First().Name);

            categoryDataAccess.Delete(category);

            list = categoryDataAccess.LoadList();
            Assert.IsFalse(list.Any());

        }

    }
}
