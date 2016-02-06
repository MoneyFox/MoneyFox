using MoneyManager.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MvvmCross.Plugins.Sqlite.WindowsUWP;
using NUnit.Framework;

namespace MoneyManager.Windows.DataAccess.Tests
{
    [TestFixture]
    public class CategoryDataAccessTests
    {
        private SqliteConnectionCreator connectionCreator;

        [SetUp]
        public void Init()
        {
            connectionCreator = new SqliteConnectionCreator(new WindowsSqliteConnectionFactory());
        }

        [Test]
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

        [Test]
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
    }
}