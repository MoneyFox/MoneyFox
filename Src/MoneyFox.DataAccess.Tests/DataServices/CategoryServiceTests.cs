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
    public class CategoryServiceTests
    {
        private readonly DbContextScopeFactory dbContextScopeFactory;
        private readonly AmbientDbContextLocator ambientDbContextLocator;

        /// <summary>
        ///     Setup Logic who is executed before every test
        /// </summary>
        public CategoryServiceTests()
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
        public async void SaveCategory_CategorySavedAndLoaded()
        {
            // Arrange
            var categoryService = new CategoryService(ambientDbContextLocator, dbContextScopeFactory);

            var testEntry = new CategoryEntity
            {
                Name = "Testtext"
            };

            // Act
            await categoryService.SaveCategory(new Category(testEntry));

            // Assert
            var loadedEntry = await categoryService.GetById(testEntry.Id);
            Assert.Equal(testEntry.Name, loadedEntry.Data.Name);
        }

        [Fact]
        public async void SaveCategory_SavedIdSet()
        {
            // Arrange
            var categoryService = new CategoryService(ambientDbContextLocator, dbContextScopeFactory);

            var testEntry = new CategoryEntity
            {
                Name = "Testtext"
            };

            // Act
            await categoryService.SaveCategory(new Category(testEntry));

            // Assert
            Assert.NotEqual(0, testEntry.Id);
        }

        [Fact]
        public async void SaveCategory_MultipleCategorySavedAndLoaded()
        {
            // Arrange
            var categoryService = new CategoryService(ambientDbContextLocator, dbContextScopeFactory);

            var testEntry1 = new CategoryEntity
            {
                Name = "Testtext"
            };
            var testEntry2 = new CategoryEntity
            {
                Name = "Testtext"
            };

            // Act
            await categoryService.SaveCategory(new Category(testEntry1));
            await categoryService.SaveCategory(new Category(testEntry2));

            // Assert
            Assert.Equal(testEntry1.Name, (await categoryService.GetById(testEntry1.Id)).Data.Name);
            Assert.Equal(testEntry1.Name, (await categoryService.GetById(testEntry2.Id)).Data.Name);
        }

        [Fact]
        public async void SaveCategory_CategoryUpdatedCorrectly()
        {
            // Arrange
            const string udpatedString = "new Category Name";
            var categoryService = new CategoryService(ambientDbContextLocator, dbContextScopeFactory);

            var testEntry = new CategoryEntity
            {
                Name = "Testtext"
            };

            await categoryService.SaveCategory(new Category(testEntry));
            testEntry.Name = udpatedString;

            // Act
            await categoryService.SaveCategory(new Category(testEntry));

            // Assert
            var loadedEntry = await categoryService.GetById(testEntry.Id);
            Assert.Equal(udpatedString, loadedEntry.Data.Name);
        }

        [Fact]
        public async void Add_NewEntryWithoutName()
        {
            // Arrange
            var categoryService = new CategoryService(ambientDbContextLocator, dbContextScopeFactory);

            // Act // Assert
            await Assert.ThrowsAsync<DbUpdateException>(async () => await categoryService.SaveCategory(new Category(new CategoryEntity())));

        }

        [Fact]
        public async void DeleteCategory_CategoryDeleted()
        {
            // Arrange
            var categoryService = new CategoryService(ambientDbContextLocator, dbContextScopeFactory);

            var testEntry = new CategoryEntity
            {
                Name = "Testtext"
            };

            await categoryService.SaveCategory(new Category(testEntry));

            // Act
            await categoryService.DeleteCategory(new Category(testEntry));

            // Assert
            Assert.Null(await categoryService.GetById(testEntry.Id));
        }

        [Fact]
        public async void DeleteCategory_PaymentCategorySetNull()
        {
            // Arrange
            var accountService = new AccountService(ambientDbContextLocator, dbContextScopeFactory);
            var categoryService = new CategoryService(ambientDbContextLocator, dbContextScopeFactory);
            var paymentService = new PaymentService(ambientDbContextLocator, dbContextScopeFactory);

            var category = new Category(new CategoryEntity
            {
                Name = "Category"
            });
            var chargedAccount = new Account(new AccountEntity
            {
                Name = "Charged"
            });

            await categoryService.SaveCategory(category);
            await accountService.SaveAccount(chargedAccount);

            var payment = new Payment(new PaymentEntity
            {
                Note = "Foo",
                ChargedAccount = chargedAccount.Data,
                Category = category.Data
            });
            await paymentService.SavePayments(payment);


            // Act
            await categoryService.DeleteCategory(category);

            // Assert
            Assert.Null((await paymentService.GetById(payment.Data.Id)).Data.Category);
        }
    }
}
