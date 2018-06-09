using System;
using System.IO;
using EntityFramework.DbContextScope;
using Microsoft.EntityFrameworkCore;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Foundation.Constants;
using Xunit;

namespace MoneyFox.DataAccess.Tests.DataServices
{
    public class CategoryGroupServiceTests
    {
        private readonly DbContextScopeFactory dbContextScopeFactory;
        private readonly AmbientDbContextLocator ambientDbContextLocator;

        /// <summary>
        ///     Setup Logic who is executed before every test
        /// </summary>
        public CategoryGroupServiceTests()
        {
            ApplicationContext.DbPath = Path.Combine(AppContext.BaseDirectory, DatabaseConstants.DB_NAME);
            Dispose();

            dbContextScopeFactory = new DbContextScopeFactory();
            ambientDbContextLocator = new AmbientDbContextLocator();

            using (dbContextScopeFactory.Create())
            {
                ambientDbContextLocator.Get<ApplicationContext>().Database.Migrate();
            }
        }

        /// <summary>
        ///     Cleanup logic who is executed after executign every test.
        /// </summary>
        public void Dispose()
        {
            if (File.Exists(ApplicationContext.DbPath))
            {
                File.Delete(ApplicationContext.DbPath);
            }
        }

        [Fact]
        public async void SaveGroup_CategorySavedAndLoaded()
        {
            // Arrange
            var categoryGroupService = new CategoryGroupService(ambientDbContextLocator, dbContextScopeFactory);

            var testEntry = new CategoryGroupEntity
            {
                Name = "Testtext"
            };

            // Act
            await categoryGroupService.SaveGroup(new CategoryGroup(testEntry));

            // Assert
            var loadedEntry = await categoryGroupService.GetById(testEntry.Id);
            Assert.Equal(testEntry.Name, loadedEntry.Data.Name);
        }

        [Fact]
        public async void SaveGroup_SavedIdSet()
        {
            // Arrange
            var categoryGroupService = new CategoryGroupService(ambientDbContextLocator, dbContextScopeFactory);

            var testEntry = new CategoryGroupEntity
            {
                Name = "Testtext"
            };

            // Act
            await categoryGroupService.SaveGroup(new CategoryGroup(testEntry));

            // Assert
            Assert.NotEqual(0, testEntry.Id);
        }

        [Fact]
        public async void SaveGroup_MultipleGroupsSavedAndLoaded()
        {
            // Arrange
            var categoryGroupService = new CategoryGroupService(ambientDbContextLocator, dbContextScopeFactory);

            var testEntry1 = new CategoryGroupEntity
            {
                Name = "Testtext"
            };
            var testEntry2 = new CategoryGroupEntity
            {
                Name = "Testtext"
            };

            // Act
            await categoryGroupService.SaveGroup(new CategoryGroup(testEntry1));
            await categoryGroupService.SaveGroup(new CategoryGroup(testEntry2));

            // Assert
            Assert.Equal(testEntry1.Name, (await categoryGroupService.GetById(testEntry1.Id)).Data.Name);
            Assert.Equal(testEntry1.Name, (await categoryGroupService.GetById(testEntry2.Id)).Data.Name);
        }

        [Fact]
        public async void SaveGroup_GroupUpdatedCorrectly()
        {
            // Arrange
            const string udpatedString = "new CategoryGroup Name";
            var categoryGroupService = new CategoryGroupService(ambientDbContextLocator, dbContextScopeFactory);

            var testEntry = new CategoryGroupEntity
            {
                Name = "Testtext"
            };

            await categoryGroupService.SaveGroup(new CategoryGroup(testEntry));
            testEntry.Name = udpatedString;

            // Act
            await categoryGroupService.SaveGroup(new CategoryGroup(testEntry));

            // Assert
            var loadedEntry = await categoryGroupService.GetById(testEntry.Id);
            Assert.Equal(udpatedString, loadedEntry.Data.Name);
        }

        [Fact]
        public async void Add_NewEntryWithoutName()
        {
            // Arrange
            var categoryGroupService = new CategoryGroupService(ambientDbContextLocator, dbContextScopeFactory);

            // Act // Assert
            await Assert.ThrowsAsync<DbUpdateException>(async () => await categoryGroupService.SaveGroup(new CategoryGroup(new CategoryGroupEntity())));

        }

        [Fact]
        public async void DeleteCategory_CategoryDeleted()
        {
            // Arrange
            var CategoryGroupService = new CategoryGroupService(ambientDbContextLocator, dbContextScopeFactory);

            var testEntry = new CategoryGroupEntity
            {
                Name = "Testtext"
            };

            await CategoryGroupService.SaveGroup(new CategoryGroup(testEntry));

            // Act
            await CategoryGroupService.DeleteGroup(new CategoryGroup(testEntry));

            // Assert
            Assert.Null(await CategoryGroupService.GetById(testEntry.Id));
        }

        [Fact]
        public async void DeleteCategory_PaymentCategorySetNull()
        {
            // Arrange
            var categoryGroupService = new CategoryGroupService(ambientDbContextLocator, dbContextScopeFactory);
            var categoryService = new CategoryService(ambientDbContextLocator, dbContextScopeFactory);

            var categoryGroup = new CategoryGroup(new CategoryGroupEntity
            {
                Name = "CategoryGroup"
            });
            await categoryGroupService.SaveGroup(categoryGroup);

            var category = new Category(new CategoryEntity
            {
                Name = "Charged",
                Group = categoryGroup.Data
            });
            await categoryService.SaveCategory(category);


            // Act
            await categoryGroupService.DeleteGroup(categoryGroup);

            // Assert
            Assert.Null((await categoryService.GetById(category.Data.Id)).Data.Group);
        }
    }
}
