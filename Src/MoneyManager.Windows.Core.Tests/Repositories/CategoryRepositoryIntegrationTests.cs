using System.Linq;
using MoneyManager.Core;
using MoneyManager.Core.DataAccess;
using MoneyManager.Core.Repositories;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Windows.Core.Tests.Helper;
using SQLite.Net.Platform.WinRT;
using SQLiteNetExtensions.Extensions;
using Xunit;

namespace MoneyManager.Windows.Core.Tests.Repositories
{
    public class CategoryRepositoryIntegrationTests
    {
        [Fact]
        [Trait("Category", "Integration")]
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

            repository.Data.Any().ShouldBeTrue();
            repository.Data[0].Name.ShouldBe("Foooo");
        }

        [Fact]
        [Trait("Category", "Integration")]
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

            repository.Data.Count.ShouldBe(1);
            repository.Data[0].ShouldBeSameAs(category);

            category.Name = "newName";

            repository.Save(category);

            repository.Data.Count.ShouldBe(1);
            repository.Data[0].Name.ShouldBe("newName");
        }
    }
}