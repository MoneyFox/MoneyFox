using MoneyManager.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MvvmCross.Plugins.Sqlite.WindowsUWP;
using Xunit;
using XunitShouldExtension;

namespace MoneyManager.Windows.DataAccess.Tests
{
    public class CategoryDataAccessTests
    {
        [Fact]
        public void SaveToDatabase_NewCategory_CorrectId()
        {
            var name = "TestCategory";

            var category = new Category
            {
                Name = name
            };

            new CategoryDataAccess(new SqliteConnectionCreator(new WindowsSqliteConnectionFactory())).SaveItem(category);

            category.Id.ShouldBeGreaterThanOrEqualTo(1);
            category.Name.ShouldBe(name);
        }

        [Fact]
        public void SaveToDatabase_ExistingCategory_CorrectId()
        {
            var category = new Category();

            var dataAccess = new CategoryDataAccess(new SqliteConnectionCreator(new WindowsSqliteConnectionFactory()));
            dataAccess.SaveItem(category);

            category.Name.ShouldBeNull();
            var id = category.Id;

            var name = "TestCategory";
            category.Name = name;

            category.Id.ShouldBe(id);
            category.Name.ShouldBe(name);
        }
    }
}