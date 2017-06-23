using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.Infrastructure;
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
        public async void Add_NewEntryWithoutName()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new AccountRepository(factory);

            var testEntry = new AccountEntity();

            // Act // Assert
            repository.Add(testEntry);
            await Assert.ThrowsAsync<DbUpdateException>(async () => await unitOfWork.Commit());
        }

        [Fact]
        public async void Add_AddedAndRead()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new AccountRepository(factory);

            var testEntry = new AccountEntity
            {
                Name = "Testtext"
            };

            // Act
            repository.Add(testEntry);
            await unitOfWork.Commit();

            // Assert
            var loadedEntry = await repository.GetById(testEntry.Id);
            Assert.Equal(testEntry.Name, loadedEntry.Name);
        }

        [Fact]
        public async void Add_AddMultipleEntries()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new AccountRepository(factory);

            // Act
            repository.Add(new AccountEntity { Name = "testAccount" });
            repository.Add(new AccountEntity { Name = "testAccount" });
            repository.Add(new AccountEntity { Name = "testAccount" });
            await unitOfWork.Commit();

            // Assert
            Assert.Equal(3, repository.GetAll().Count());
        }

        [Fact]
        public async void Add_AddNewEntryOnEveryCall()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new AccountRepository(factory);

            var testEntry = new AccountEntity
            {
                Name = "Testtext"
            };

            // Act
            repository.Add(testEntry);
            await unitOfWork.Commit();
            testEntry.Id = 0;
            repository.Add(testEntry);
            await unitOfWork.Commit();

            // Assert
            Assert.Equal(2, repository.GetAll().Count());
        }

        [Fact]
        public async void Add_IdSet()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new AccountRepository(factory);

            var testEntry = new AccountEntity
            {
                Name = "Testtext"
            };

            // Act
            repository.Add(testEntry);
            await unitOfWork.Commit();

            // Assert
            Assert.NotNull(testEntry.Id);
            Assert.NotEqual(0, testEntry.Id);
        }

        [Fact]
        public async void Update_EntryUpdated()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new AccountRepository(factory);

            var newValue = "newText";
            var testEntry = new AccountEntity
            {
                Name = "Testtext"
            };

            // Act
            repository.Add(testEntry);
            await unitOfWork.Commit();

            testEntry.Name = newValue;
            repository.Update(testEntry);
            await unitOfWork.Commit();

            // Assert
            var loadedEntry = await repository.GetById(testEntry.Id);
            Assert.Equal(newValue, loadedEntry.Name);
        }

        [Fact]
        public async void Update_IdUnchanged()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new AccountRepository(factory);

            var testEntry = new AccountEntity
            {
                Name = "Testtext"
            };

            // Act
            repository.Add(testEntry);
            await unitOfWork.Commit();

            var idBeforeUpdate = testEntry.Id;
            repository.Update(testEntry);
            await unitOfWork.Commit();

            // Assert
            Assert.Equal(idBeforeUpdate, testEntry.Id);
        }

        [Fact]
        public async void Update_NoNewEntryAdded()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new AccountRepository(factory);

            var testEntry = new AccountEntity
            {
                Name = "Testtext"
            };

            // Act
            repository.Add(testEntry);
            await unitOfWork.Commit();

            repository.Update(testEntry);
            await unitOfWork.Commit();

            // Assert
            Assert.Equal(1, repository.GetAll().Count());
        }

        [Fact]
        public async void Delete_EntryDeleted()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new AccountRepository(factory);
            var testEntry = new AccountEntity { Name = "testAccount" };
            repository.Add(testEntry);
            await unitOfWork.Commit();

            // Act
            repository.Delete(testEntry);
            await unitOfWork.Commit();

            // Assert
            Assert.Equal(0, repository.GetAll().Count());
        }

        [Fact]
        public async void DeleteAccount_RelatedChargedPaymentsRemoved()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var accountRepository = new AccountRepository(factory);
            var paymentRepository = new PaymentRepository(factory);

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
            await unitOfWork.Commit();

            Assert.Equal(1, await accountRepository.GetAll().CountAsync());
            Assert.Equal(1, await paymentRepository.GetAll().CountAsync());

            // Act
            accountRepository.Delete(account);
            await unitOfWork.Commit();

            // Assert
            Assert.False(await accountRepository.GetAll().AnyAsync());
            Assert.False(await paymentRepository.GetAll().AnyAsync());
        }

        [Fact]
        public async void DeleteAccount_RelatedTargetPaymentSetNull()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var accountRepository = new AccountRepository(factory);
            var paymentRepository = new PaymentRepository(factory);

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

            accountRepository.Add(chargedAccount);
            accountRepository.Add(targetAccount);
            paymentRepository.Add(payment);
            await unitOfWork.Commit();

            Assert.Equal(2, await accountRepository.GetAll().CountAsync());
            Assert.Equal(1, await paymentRepository.GetAll().CountAsync());

            // Act
            accountRepository.Delete(targetAccount);
            await unitOfWork.Commit();

            // Assert
            Assert.Null(payment.TargetAccount);
            Assert.NotNull(payment.ChargedAccount);
        }

        [Fact]
        public async void Delete_EntryNotFound()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new AccountRepository(factory);
            var testEntry = new AccountEntity { Name = "testAccount" };

            // Act
            repository.Delete(testEntry);

            // Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await unitOfWork.Commit());
        }


        [Fact]
        public async void Delete_EntryMatchedFilterDeleted()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var filterText = "Text";
            var repository = new AccountRepository(factory);
            var testEntry1 = new AccountEntity { Name = filterText };
            var testEntry2 = new AccountEntity { Name = "testAccount" };
            repository.Add(testEntry1);
            repository.Add(testEntry2);
            await unitOfWork.Commit();

            // Act
            repository.Delete(x => x.Name == filterText);
            await unitOfWork.Commit();

            // Assert
            Assert.Equal(1, repository.GetAll().Count());
        }

        [Fact]
        public void GetAll_NoData()
        {
            // Arrange
            var repository = new AccountRepository(new DbFactory());

            // Act
            var emptyList = repository.GetAll().ToList();

            // Assert
            Assert.NotNull(emptyList);
            Assert.False(emptyList.Any());
        }

        [Fact]
        public async void GetAll_AllDataReturned()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new AccountRepository(factory);
            repository.Add(new AccountEntity { Name = "testAccount" });
            repository.Add(new AccountEntity { Name = "testAccount" });
            repository.Add(new AccountEntity { Name = "testAccount" });
            await unitOfWork.Commit();

            // Act
            var resultList = repository.GetAll().ToList();

            // Assert
            Assert.NotNull(resultList);
            Assert.Equal(3, resultList.Count);
        }

        [Fact]
        public async void GetMany_NothingMatched()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new AccountRepository(factory);
            repository.Add(new AccountEntity { Name = "testAccount" });
            repository.Add(new AccountEntity { Name = "testAccount" });
            repository.Add(new AccountEntity { Name = "testAccount" });
            await unitOfWork.Commit();

            // Act
            var resultList = repository.GetMany(x => x.Name == "text").ToList();

            // Assert
            Assert.NotNull(resultList);
            Assert.False(resultList.Any());
        }

        [Fact]
        public async void GetMany_MatchedDataReturned()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new AccountRepository(factory);
            var filterText = "Text";
            repository.Add(new AccountEntity { Name = filterText });
            repository.Add(new AccountEntity { Name = "testAccount" });
            repository.Add(new AccountEntity { Name = "testAccount" });
            await unitOfWork.Commit();

            // Act
            var resultList = repository.GetMany(x => x.Name == filterText).ToList();

            // Assert
            Assert.NotNull(resultList);
            Assert.Equal(1, resultList.Count);
        }

        [Fact]
        public async void Get_NothingMatched()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new AccountRepository(factory);
            repository.Add(new AccountEntity { Name = "testAccount" });
            repository.Add(new AccountEntity { Name = "testAccount" });
            repository.Add(new AccountEntity { Name = "testAccount" });
            await unitOfWork.Commit();

            // Act
            var result = await repository.Get(x => x.Name == "text");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void Get_MatchedDataReturned()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new AccountRepository(factory);
            var filterText = "Text";
            var testEntry = new AccountEntity { Name = filterText };
            repository.Add(testEntry);
            repository.Add(new AccountEntity { Name = "testAccount" });
            repository.Add(new AccountEntity { Name = "testAccount" });
            await unitOfWork.Commit();

            // Act
            var result = await repository.Get(x => x.Name == filterText);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testEntry.Id, result.Id);
        }
    }
}