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
    public class AccountRepositoryTests : IDisposable
    {
        /// <summary>
        ///     Setup Logic who is executed before every test
        /// </summary>
        public AccountRepositoryTests()
        {
            var dbContextScopeFactory = new DbContextScopeFactory();
            var ambientDbContextLocator = new AmbientDbContextLocator();

            using (dbContextScopeFactory.Create())
            {
                ambientDbContextLocator.Get<ApplicationContext>().Database.Migrate();
            }

            ApplicationContext.DbPath = Path.Combine(AppContext.BaseDirectory, DatabaseConstants.DB_NAME);
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
            var dbContextScopeFactory = new DbContextScopeFactory();
            var ambientDbContextLocator = new AmbientDbContextLocator();

            var repository = new AccountRepository(ambientDbContextLocator);

            var testEntry = new AccountEntity
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
            var loadedEntry = await repository.GetById(testEntry.Id);
            Assert.Equal(testEntry.Name, loadedEntry.Name);
        }

        [Fact]
        public async void Add_AddMultipleEntries()
        {
            // Arrange
            var dbContextScopeFactory = new DbContextScopeFactory();
            var ambientDbContextLocator = new AmbientDbContextLocator();

            var repository = new AccountRepository(ambientDbContextLocator);

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Add(new AccountEntity {Name = "testAccount"});
                repository.Add(new AccountEntity {Name = "testAccount"});
                repository.Add(new AccountEntity {Name = "testAccount"});
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            Assert.Equal(3, repository.GetAll().Count());
        }

        [Fact]
        public async void Add_AddNewEntryOnEveryCall()
        {
            // Arrange
            var dbContextScopeFactory = new DbContextScopeFactory();
            var ambientDbContextLocator = new AmbientDbContextLocator();

            var repository = new AccountRepository(ambientDbContextLocator);

            var testEntry = new AccountEntity
            {
                Name = "Testtext"
            };

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Add(testEntry);
                await dbContextScope.SaveChangesAsync();
                testEntry.Id = 0;
                repository.Add(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            Assert.Equal(2, repository.GetAll().Count());
        }

        [Fact]
        public async void Add_IdSet()
        {
            // Arrange
            var dbContextScopeFactory = new DbContextScopeFactory();
            var ambientDbContextLocator = new AmbientDbContextLocator();

            var repository = new AccountRepository(ambientDbContextLocator);

            var testEntry = new AccountEntity
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
            var dbContextScopeFactory = new DbContextScopeFactory();
            var ambientDbContextLocator = new AmbientDbContextLocator();

            var repository = new AccountRepository(ambientDbContextLocator);

            var testEntry = new AccountEntity();

            // Act // Assert
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Add(testEntry);
                await Assert.ThrowsAsync<DbUpdateException>(async () => await dbContextScope.SaveChangesAsync());
            }
        }

        [Fact]
        public async void Delete_EntryDeleted()
        {
            // Arrange
            var dbContextScopeFactory = new DbContextScopeFactory();
            var ambientDbContextLocator = new AmbientDbContextLocator();

            var repository = new AccountRepository(ambientDbContextLocator);

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                var testEntry = new AccountEntity {Name = "testAccount"};
                repository.Add(testEntry);
                await dbContextScope.SaveChangesAsync();

                repository.Delete(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            Assert.Equal(0, repository.GetAll().Count());
        }


        [Fact]
        public async void Delete_EntryMatchedFilterDeleted()
        {
            // Arrange
            var dbContextScopeFactory = new DbContextScopeFactory();
            var ambientDbContextLocator = new AmbientDbContextLocator();

            var filterText = "Text";
            var repository = new AccountRepository(ambientDbContextLocator);
            var testEntry1 = new AccountEntity {Name = filterText};
            var testEntry2 = new AccountEntity {Name = "testAccount"};

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Add(testEntry1);
                repository.Add(testEntry2);
                await dbContextScope.SaveChangesAsync();

                repository.Delete(x => x.Name == filterText);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            Assert.Equal(1, repository.GetAll().Count());
        }

        [Fact]
        public async void Delete_EntryNotFound()
        {
            // Arrange
            var dbContextScopeFactory = new DbContextScopeFactory();
            var ambientDbContextLocator = new AmbientDbContextLocator();

            var repository = new AccountRepository(ambientDbContextLocator);
            var testEntry = new AccountEntity {Name = "testAccount"};

            // Act / Assert
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Delete(testEntry);
                await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await dbContextScope.SaveChangesAsync());
            }
        }

        [Fact]
        public async void DeleteAccount_RelatedChargedPaymentsRemoved()
        {
            // Arrange
            var dbContextScopeFactory = new DbContextScopeFactory();
            var ambientDbContextLocator = new AmbientDbContextLocator();

            var accountRepository = new AccountRepository(ambientDbContextLocator);
            var paymentRepository = new PaymentRepository(ambientDbContextLocator);

            Assert.Equal(1, await accountRepository.GetAll().CountAsync());
            Assert.Equal(1, await paymentRepository.GetAll().CountAsync());

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                var account = new AccountEntity
                {
                    Name = "Testtext"
                };

                var payment = new PaymentEntity
                {
                    Note = "Foo",
                    ChargedAccount = account
                };

                accountRepository.Add(account);
                paymentRepository.Add(payment);
                await dbContextScope.SaveChangesAsync();

                accountRepository.Delete(account);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            Assert.False(await accountRepository.GetAll().AnyAsync());
            Assert.False(await paymentRepository.GetAll().AnyAsync());
        }

        [Fact]
        public async void DeleteAccount_RelatedTargetPaymentSetNull()
        {
            // Arrange
            var dbContextScopeFactory = new DbContextScopeFactory();
            var ambientDbContextLocator = new AmbientDbContextLocator();

            var accountRepository = new AccountRepository(ambientDbContextLocator);
            var paymentRepository = new PaymentRepository(ambientDbContextLocator);

            var chargedAccount = new AccountEntity
            {
                Name = "Charged"
            };
            var targetAccount = new AccountEntity
            {
                Name = "Target"
            };

            var payment = new PaymentEntity
            {
                Note = "Foo",
                ChargedAccount = chargedAccount,
                TargetAccount = targetAccount
            };

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                accountRepository.Add(chargedAccount);
                accountRepository.Add(targetAccount);
                paymentRepository.Add(payment);
                await dbContextScope.SaveChangesAsync();

                Assert.Equal(2, await accountRepository.GetAll().CountAsync());
                Assert.Equal(1, await paymentRepository.GetAll().CountAsync());

                accountRepository.Delete(targetAccount);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            Assert.Null(payment.TargetAccount);
            Assert.NotNull(payment.ChargedAccount);
        }

        [Fact]
        public async void Get_MatchedDataReturned()
        {
            // Arrange
            var dbContextScopeFactory = new DbContextScopeFactory();
            var ambientDbContextLocator = new AmbientDbContextLocator();

            var filterText = "Text";
            var testEntry = new AccountEntity { Name = filterText };

            AccountEntity result;

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                var repository = new AccountRepository(ambientDbContextLocator);
                repository.Add(testEntry);
                repository.Add(new AccountEntity { Name = "testAccount" });
                repository.Add(new AccountEntity { Name = "testAccount" });
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
            var dbContextScopeFactory = new DbContextScopeFactory();
            var ambientDbContextLocator = new AmbientDbContextLocator();

            AccountEntity result;


            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                var repository = new AccountRepository(ambientDbContextLocator);
                repository.Add(new AccountEntity {Name = "testAccount"});
                repository.Add(new AccountEntity {Name = "testAccount"});
                repository.Add(new AccountEntity {Name = "testAccount"});
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
            var dbContextScopeFactory = new DbContextScopeFactory();
            var ambientDbContextLocator = new AmbientDbContextLocator();

            List<AccountEntity> resultList;

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                var repository = new AccountRepository(ambientDbContextLocator);
                repository.Add(new AccountEntity {Name = "testAccount"});
                repository.Add(new AccountEntity {Name = "testAccount"});
                repository.Add(new AccountEntity {Name = "testAccount"});
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
            var dbContextScopeFactory = new DbContextScopeFactory();
            var ambientDbContextLocator = new AmbientDbContextLocator();


            var repository = new AccountRepository(ambientDbContextLocator);

            // Act
            var emptyList = repository.GetAll().ToList();

            // Assert
            Assert.NotNull(emptyList);
            Assert.False(emptyList.Any());
        }

        [Fact]
        public async void GetMany_MatchedDataReturned()
        {
            // Arrange
            var dbContextScopeFactory = new DbContextScopeFactory();
            var ambientDbContextLocator = new AmbientDbContextLocator();

            List<AccountEntity> resultList;

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                var repository = new AccountRepository(ambientDbContextLocator);
                var filterText = "Text";
                repository.Add(new AccountEntity {Name = filterText});
                repository.Add(new AccountEntity {Name = "testAccount"});
                repository.Add(new AccountEntity {Name = "testAccount"});
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
            var dbContextScopeFactory = new DbContextScopeFactory();
            var ambientDbContextLocator = new AmbientDbContextLocator();

            List<AccountEntity> resultList;

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                var repository = new AccountRepository(ambientDbContextLocator);
                repository.Add(new AccountEntity {Name = "testAccount"});
                repository.Add(new AccountEntity {Name = "testAccount"});
                repository.Add(new AccountEntity {Name = "testAccount"});
                await dbContextScope.SaveChangesAsync();
                resultList = repository.GetMany(x => x.Name == "text").ToList();
            }

            // Assert
            Assert.NotNull(resultList);
            Assert.False(resultList.Any());
        }

        [Fact]
        public async void GetName_NameReturned()
        {
            // Arrange
            var dbContextScopeFactory = new DbContextScopeFactory();
            var ambientDbContextLocator = new AmbientDbContextLocator();
            const string accountName = "TestAccount";

            string result;

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                var repository = new AccountRepository(ambientDbContextLocator);

                var testEntry = new AccountEntity { Name = accountName };
                repository.Add(testEntry);
                await dbContextScope.SaveChangesAsync();

                 result = await repository.GetName(testEntry.Id);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal(accountName, result);
        }

        [Fact]
        public async void Update_EntryUpdated()
        {
            // Arrange
            var dbContextScopeFactory = new DbContextScopeFactory();
            var ambientDbContextLocator = new AmbientDbContextLocator();

            var repository = new AccountRepository(ambientDbContextLocator);

            var newValue = "newText";
            var testEntry = new AccountEntity
            {
                Name = "Testtext"
            };

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Add(testEntry);
                await dbContextScope.SaveChangesAsync();

                testEntry.Name = newValue;
                repository.Update(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            var loadedEntry = await repository.GetById(testEntry.Id);
            Assert.Equal(newValue, loadedEntry.Name);
        }

        [Fact]
        public async void Update_IdUnchanged()
        {
            // Arrange
            var dbContextScopeFactory = new DbContextScopeFactory();
            var ambientDbContextLocator = new AmbientDbContextLocator();

            var repository = new AccountRepository(ambientDbContextLocator);

            var testEntry = new AccountEntity
            {
                Name = "Testtext"
            };

            int idBeforeUpdate;

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Add(testEntry);
                await dbContextScope.SaveChangesAsync();

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
            var dbContextScopeFactory = new DbContextScopeFactory();
            var ambientDbContextLocator = new AmbientDbContextLocator();

            var repository = new AccountRepository(ambientDbContextLocator);

            var testEntry = new AccountEntity
            {
                Name = "Testtext"
            };

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Add(testEntry);
                await dbContextScope.SaveChangesAsync();

                repository.Update(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            Assert.Equal(1, repository.GetAll().Count());
        }
    }
}