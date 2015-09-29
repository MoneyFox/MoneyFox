using System.Linq;
using MoneyManager.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MvvmCross.Plugins.Sqlite.WindowsUWP;
using Xunit;

namespace MoneyManager.Windows.Core.Tests.DataAccess
{
    public class CategoryDataAccessTest
    {
        [Fact]
        [Trait("Category", "Integration")]
        public void CategoryDataAccess_CrudCategory()
        {
            var categoryDataAccess =
                new CategoryDataAccess(new SqliteConnectionCreator(new WindowsSqliteConnectionFactory()));

            const string firstName = "category";
            const string secondName = "new category";

            var category = new Category
            {
                Name = firstName
            };

            categoryDataAccess.SaveItem(category);

            categoryDataAccess.LoadList();
            var list = categoryDataAccess.LoadList();

            Assert.Equal(1, list.Count);
            Assert.Equal(firstName, list.First().Name);

            category.Name = secondName;
            categoryDataAccess.SaveItem(category);

            list = categoryDataAccess.LoadList();

            Assert.Equal(1, list.Count);
            Assert.Equal(secondName, list.First().Name);

            categoryDataAccess.DeleteItem(category);

            list = categoryDataAccess.LoadList();
            Assert.False(list.Any());
        }
    }
}