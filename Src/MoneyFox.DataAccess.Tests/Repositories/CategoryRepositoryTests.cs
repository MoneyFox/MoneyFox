using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EntityFramework.DbContextScope;
using Microsoft.EntityFrameworkCore;
using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.Repositories;
using MoneyFox.Foundation.Constants;
using Xunit;

namespace MoneyFox.DataAccess.Tests.Repositories
{
    public class CategoryRepositoryTests : IDisposable
    {
        private DbContextScopeFactory dbContextScopeFactory;
        private AmbientDbContextLocator ambientDbContextLocator;

        /// <summary>
        ///     Setup Logic who is executed before every test
        /// </summary>
        public CategoryRepositoryTests()
        {
            ApplicationContext.DbPath = Path.Combine(AppContext.BaseDirectory, DatabaseConstants.DB_NAME);

            dbContextScopeFactory = new DbContextScopeFactory();
            ambientDbContextLocator = new AmbientDbContextLocator();

            using (dbContextScopeFactory.Create())
            {
                ambientDbContextLocator.Get<ApplicationContext>().Database.Migrate();
            }

            using (var db = new ApplicationContext())
            {
                db.Database.Migrate();
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
        public async void Add_AddedAndRead()
        {
            // Arrange
            var repository = new CategoryRepository(ambientDbContextLocator);

            var testEntry = new CategoryEntity
            {
                Name = "Testtext"
            };

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Add(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            using (dbContextScopeFactory.CreateReadOnly())
            {
                var loadedEntry = await repository.GetById(testEntry.Id);
                Assert.Equal(testEntry.Name, loadedEntry.Name);
            }
        }

        [Fact]
        public async void Add_AddMultipleEntries()
        {
            // Arrange
            var repository = new CategoryRepository(ambientDbContextLocator);

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Add(new CategoryEntity {Name = "TestCategory"});
                repository.Add(new CategoryEntity {Name = "TestCategory"});
                repository.Add(new CategoryEntity {Name = "TestCategory"});
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            using (dbContextScopeFactory.CreateReadOnly())
            {
                Assert.Equal(3, repository.GetAll().Count());
            }
        }

        [Fact]
        public async void Add_AddNewEntryOnEveryCall()
        {
            // Arrange
            var repository = new CategoryRepository(ambientDbContextLocator);

            var testEntry = new CategoryEntity
            {
                Name = "Testtext"
            };
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Add(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                testEntry.Id = 0;
                repository.Add(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            using (dbContextScopeFactory.CreateReadOnly())
            {
                Assert.Equal(2, repository.GetAll().Count());
            }
        }

        [Fact]
        public async void Add_IdSet()
        {
            // Arrange
            var repository = new CategoryRepository(ambientDbContextLocator);

            var testEntry = new CategoryEntity
            {
                Name = "Testtext"
            };

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Add(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            Assert.NotNull(testEntry.Id);
            Assert.NotEqual(0, testEntry.Id);
        }

        [Fact]
        public async void Add_NewEntryWithoutName()
        {
            // Arrange
            var repository = new CategoryRepository(ambientDbContextLocator);

            var testEntry = new CategoryEntity();

            // Act // Assert
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Add(testEntry);
                await Assert.ThrowsAsync<DbUpdateException>(async () => await dbContextScope.SaveChangesAsync());
            }
        }

        [Fact]
        public async void Delete_AssignedPaymentsSetNull()
        {
            // Arrange
            var categoryRepository = new CategoryRepository(ambientDbContextLocator);
            var accountRepository = new AccountRepository(ambientDbContextLocator);
            var paymentRepository = new PaymentRepository(ambientDbContextLocator);

            var category = new CategoryEntity {Name = "TestCategory"};
            var account = new AccountEntity { Name = "testAccount" };
            
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                categoryRepository.Add(category);
                accountRepository.Add(account);
                await dbContextScope.SaveChangesAsync();
            }

            var payment = new PaymentEntity
            {
                ChargedAccount = account,
                Category = category
            };
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                paymentRepository.Add(payment);
                await dbContextScope.SaveChangesAsync();
            }

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {

                categoryRepository.Delete(category);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            using (dbContextScopeFactory.CreateReadOnly())
            {
                Assert.Null(payment.Category);
                Assert.Null(paymentRepository.GetById(payment.Id).Result.Category);
            }
        }

        [Fact]
        public async void Delete_AssignedRelatedPaymentsSetNull()
        {
            // Arrange
            var categoryRepository = new CategoryRepository(ambientDbContextLocator);
            var accountRepository = new AccountRepository(ambientDbContextLocator);
            var paymentRepository = new PaymentRepository(ambientDbContextLocator);

            var category = new CategoryEntity {Name = "TestCategory"};
            var account = new AccountEntity { Name = "testAccount" };
            var recurringPayment = new RecurringPaymentEntity
            {
                ChargedAccount = account,
                Category = category
            };

            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                categoryRepository.Add(category);
                accountRepository.Add(account);
                await dbContextScope.SaveChangesAsync();
            }

            var payment = new PaymentEntity
            {
                ChargedAccount = account,
                Category = category,
                RecurringPayment = recurringPayment
            };

            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                categoryRepository.Add(category);
                paymentRepository.Add(payment);
                await dbContextScope.SaveChangesAsync();
            }

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                categoryRepository.Delete(category);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            using (dbContextScopeFactory.CreateReadOnly())
            {
                Assert.Null(recurringPayment.Category);
                Assert.Null(paymentRepository.GetById(payment.Id).Result.RecurringPayment.Category);
            }
        }

        [Fact]
        public async void Delete_EntryDeleted()
        {
            // Arrange
            var repository = new CategoryRepository(ambientDbContextLocator);
            CategoryEntity testEntry;

            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                testEntry = new CategoryEntity {Name = "TestCategory"};
                repository.Add(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Delete(testEntry);
                await dbContextScope.SaveChangesAsync();
            }
            // Assert
            using (dbContextScopeFactory.CreateReadOnly())
            {
                Assert.Equal(0, repository.GetAll().Count());
            }
        }

        [Fact]
        public async void Delete_EntryMatchedFilterDeleted()
        {
            // Arrange
            var filterText = "Text";
            var repository = new CategoryRepository(ambientDbContextLocator);
            var testEntry1 = new CategoryEntity {Name = filterText};
            var testEntry2 = new CategoryEntity {Name = "TestCategory"};
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Add(testEntry1);
                repository.Add(testEntry2);
                await dbContextScope.SaveChangesAsync();
            }

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Delete(x => x.Name == filterText);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            using (dbContextScopeFactory.CreateReadOnly())
            {
                Assert.Equal(1, repository.GetAll().Count());
            }
        }

        [Fact]
        public async void Delete_EntryNotFound()
        {
            // Arrange
            var repository = new CategoryRepository(ambientDbContextLocator);
            var testEntry = new CategoryEntity {Name = "TestCategory"};

            // Act / Assert
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Delete(testEntry);
                await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await dbContextScope.SaveChangesAsync());
            }
        }

        [Fact]
        public async void Get_MatchedDataReturned()
        {
            // Arrange
            var repository = new CategoryRepository(ambientDbContextLocator);
            var filterText = "Text";
            var testEntry = new CategoryEntity {Name = filterText};

            CategoryEntity result;

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Add(testEntry);
                repository.Add(new CategoryEntity {Name = "TestCategory"});
                repository.Add(new CategoryEntity {Name = "TestCategory"});
                await dbContextScope.SaveChangesAsync();

                result = await repository.Get(x => x.Name == filterText);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testEntry.Id, result.Id);
        }

        [Fact]
        public async void Get_NothingMatched()
        {
            // Arrange
            CategoryEntity result;

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                var repository = new CategoryRepository(ambientDbContextLocator);
                repository.Add(new CategoryEntity {Name = "TestCategory"});
                repository.Add(new CategoryEntity {Name = "TestCategory"});
                repository.Add(new CategoryEntity {Name = "TestCategory"});
                await dbContextScope.SaveChangesAsync();
                result = await repository.Get(x => x.Name == "text");
            }

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void GetAll_AllDataReturned()
        {
            // Arrange
            var repository = new CategoryRepository(ambientDbContextLocator);

            List<CategoryEntity> resultList;

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Add(new CategoryEntity {Name = "TestCategory"});
                repository.Add(new CategoryEntity {Name = "TestCategory"});
                repository.Add(new CategoryEntity {Name = "TestCategory"});
                await dbContextScope.SaveChangesAsync();

                resultList = repository.GetAll().ToList();
            }

            // Assert
            Assert.NotNull(resultList);
            Assert.Equal(3, resultList.Count);
        }

        [Fact]
        public void GetAll_NoData()
        {
            // Arrange
            var repository = new CategoryRepository(ambientDbContextLocator);

            List<CategoryEntity> resultList;

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                resultList = repository.GetAll().ToList();
            }

            // Assert
            Assert.NotNull(resultList);
            Assert.False(resultList.Any());
        }

        [Fact]
        public async void GetMany_MatchedDataReturned()
        {
            // Arrange
            var repository = new CategoryRepository(ambientDbContextLocator);
            var filterText = "Text";

            List<CategoryEntity> resultList;

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Add(new CategoryEntity {Name = filterText});
                repository.Add(new CategoryEntity {Name = "TestCategory"});
                repository.Add(new CategoryEntity {Name = "TestCategory"});
                await dbContextScope.SaveChangesAsync();

                resultList = repository.GetMany(x => x.Name == filterText).ToList();
            }

            // Assert
            Assert.NotNull(resultList);
            Assert.Equal(1, resultList.Count);
        }

        [Fact]
        public async void GetMany_NothingMatched()
        {
            // Arrange
            var repository = new CategoryRepository(ambientDbContextLocator);

            List<CategoryEntity> resultList;

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Add(new CategoryEntity {Name = "TestCategory"});
                repository.Add(new CategoryEntity {Name = "TestCategory"});
                repository.Add(new CategoryEntity {Name = "TestCategory"});
                await dbContextScope.SaveChangesAsync();
                resultList = repository.GetMany(x => x.Name == "text").ToList();
            }

            // Assert
            Assert.NotNull(resultList);
            Assert.False(resultList.Any());
        }

        [Fact]
        public async void Update_EntryUpdated()
        {
            // Arrange
            var repository = new CategoryRepository(ambientDbContextLocator);

            var newValue = "newText";
            var testEntry = new CategoryEntity
            {
                Name = "Testtext"
            };

            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Add(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                testEntry.Name = newValue;
                repository.Update(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            using (dbContextScopeFactory.CreateReadOnly())
            {
                var loadedEntry = await repository.GetById(testEntry.Id);
                Assert.Equal(newValue, loadedEntry.Name);
            }
        }

        [Fact]
        public async void Update_IdUnchanged()
        {
            // Arrange
            var repository = new CategoryRepository(ambientDbContextLocator);

            var testEntry = new CategoryEntity
            {
                Name = "Testtext"
            };

            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Add(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            int idBeforeUpdate;

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                idBeforeUpdate = testEntry.Id;
                repository.Update(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            Assert.Equal(idBeforeUpdate, testEntry.Id);
        }

        [Fact]
        public async void Update_NoNewEntryAdded()
        {
            // Arrange
            var repository = new CategoryRepository(ambientDbContextLocator);

            var testEntry = new CategoryEntity
            {
                Name = "Testtext"
            };

            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Add(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Update(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            using (dbContextScopeFactory.CreateReadOnly())
            {
                Assert.Equal(1, repository.GetAll().Count());
            }
        }
    }
}