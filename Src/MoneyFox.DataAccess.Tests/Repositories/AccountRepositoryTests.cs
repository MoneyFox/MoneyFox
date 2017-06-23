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

            var testEntry = new AccountEntity();

            // Act // Assert
            unitOfWork.AccountRepository.Add(testEntry);
            await Assert.ThrowsAsync<DbUpdateException>(async () => await unitOfWork.Commit());
        }

        [Fact]
        public async void Add_AddedAndRead()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var testEntry = new AccountEntity
            {
                Name = "Testtext"
            };

            // Act
            unitOfWork.AccountRepository.Add(testEntry);
            await unitOfWork.Commit();

            // Assert
            var loadedEntry = await unitOfWork.AccountRepository.GetById(testEntry.Id);
            Assert.Equal(testEntry.Name, loadedEntry.Name);
        }

        [Fact]
        public async void Add_AddMultipleEntries()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            // Act
            unitOfWork.AccountRepository.Add(new AccountEntity { Name = "testAccount"});
            unitOfWork.AccountRepository.Add(new AccountEntity { Name = "testAccount"});
            unitOfWork.AccountRepository.Add(new AccountEntity { Name = "testAccount"});
            await unitOfWork.Commit();

            // Assert
            Assert.Equal(3, unitOfWork.AccountRepository.GetAll().Count());
        }

        [Fact]
        public async void Add_AddNewEntryOnEveryCall()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var testEntry = new AccountEntity
            {
                Name = "Testtext"
            };

            // Act
            unitOfWork.AccountRepository.Add(testEntry);
            await unitOfWork.Commit();
            testEntry.Id = 0;
            unitOfWork.AccountRepository.Add(testEntry);
            await unitOfWork.Commit();

            // Assert
            Assert.Equal(2, unitOfWork.AccountRepository.GetAll().Count());
        }

        [Fact]
        public async void Add_IdSet()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var testEntry = new AccountEntity
            {
                Name = "Testtext"
            };

            // Act
            unitOfWork.AccountRepository.Add(testEntry);
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

            var newValue = "newText";
            var testEntry = new AccountEntity
            {
                Name = "Testtext"
            };

            // Act
            unitOfWork.AccountRepository.Add(testEntry);
            await unitOfWork.Commit();

            testEntry.Name = newValue;
            unitOfWork.AccountRepository.Update(testEntry);
            await unitOfWork.Commit();

            // Assert
            var loadedEntry = await unitOfWork.AccountRepository.GetById(testEntry.Id);
            Assert.Equal(newValue, loadedEntry.Name);
        }

        [Fact]
        public async void Update_IdUnchanged()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var testEntry = new AccountEntity
            {
                Name = "Testtext"
            };

            // Act
            unitOfWork.AccountRepository.Add(testEntry);
            await unitOfWork.Commit();

            var idBeforeUpdate = testEntry.Id;
            unitOfWork.AccountRepository.Update(testEntry);
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

            var testEntry = new AccountEntity
            {
                Name = "Testtext"
            };

            // Act
            unitOfWork.AccountRepository.Add(testEntry);
            await unitOfWork.Commit();

            unitOfWork.AccountRepository.Update(testEntry);
            await unitOfWork.Commit();

            // Assert
            Assert.Equal(1, unitOfWork.AccountRepository.GetAll().Count());
        }

        [Fact]
        public async void Delete_EntryDeleted()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var testEntry = new AccountEntity { Name = "testAccount"};
            unitOfWork.AccountRepository.Add(testEntry);
            await unitOfWork.Commit();

            // Act
            unitOfWork.AccountRepository.Delete(testEntry);
            await unitOfWork.Commit();

            // Assert
            Assert.Equal(0, unitOfWork.AccountRepository.GetAll().Count());
        }

        [Fact]
        public async void DeleteAccount_RelatedChargedPaymentsRemoved()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var account = new AccountEntity
            {
                Name = "Testtext"
            };

            var payment = new PaymentEntity
            {
                Note = "Foo",
                ChargedAccount = account
            };

            unitOfWork.AccountRepository.Add(account);
            unitOfWork.PaymentRepository.Add(payment);
            await unitOfWork.Commit();

            Assert.Equal(1, await unitOfWork.AccountRepository.GetAll().CountAsync());
            Assert.Equal(1, await unitOfWork.PaymentRepository.GetAll().CountAsync());

            // Act
            unitOfWork.AccountRepository.Delete(account);
            await unitOfWork.Commit();

            // Assert
            Assert.False(await unitOfWork.AccountRepository.GetAll().AnyAsync());
            Assert.False(await unitOfWork.PaymentRepository.GetAll().AnyAsync());
        }

        [Fact]
        public async void DeleteAccount_RelatedTargetPaymentSetNull()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

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

            unitOfWork.AccountRepository.Add(chargedAccount);
            unitOfWork.AccountRepository.Add(targetAccount);
            unitOfWork.PaymentRepository.Add(payment);
            await unitOfWork.Commit();

            Assert.Equal(2, await unitOfWork.AccountRepository.GetAll().CountAsync());
            Assert.Equal(1, await unitOfWork.PaymentRepository.GetAll().CountAsync());

            // Act
            unitOfWork.AccountRepository.Delete(targetAccount);
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

            var testEntry = new AccountEntity { Name = "testAccount"};

            // Act
            unitOfWork.AccountRepository.Delete(testEntry);

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
            var testEntry1 = new AccountEntity { Name = filterText };
            var testEntry2 = new AccountEntity { Name = "testAccount"};
            unitOfWork.AccountRepository.Add(testEntry1);
            unitOfWork.AccountRepository.Add(testEntry2);
            await unitOfWork.Commit();

            // Act
            unitOfWork.AccountRepository.Delete(x => x.Name == filterText);
            await unitOfWork.Commit();

            // Assert
            Assert.Equal(1, unitOfWork.AccountRepository.GetAll().Count());
        }

        [Fact]
        public void GetAll_NoData()
        {
            // Arrange
            var unitOfWork = new UnitOfWork(new DbFactory());

            // Act
            var emptyList = unitOfWork.AccountRepository.GetAll().ToList();

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

            unitOfWork.AccountRepository.Add(new AccountEntity { Name = "testAccount"});
            unitOfWork.AccountRepository.Add(new AccountEntity { Name = "testAccount"});
            unitOfWork.AccountRepository.Add(new AccountEntity { Name = "testAccount"});
            await unitOfWork.Commit();

            // Act
            var resultList = unitOfWork.AccountRepository.GetAll().ToList();

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

            unitOfWork.AccountRepository.Add(new AccountEntity { Name = "testAccount"});
            unitOfWork.AccountRepository.Add(new AccountEntity { Name = "testAccount"});
            unitOfWork.AccountRepository.Add(new AccountEntity { Name = "testAccount"});
            await unitOfWork.Commit();

            // Act
            var resultList = unitOfWork.AccountRepository.GetMany(x => x.Name == "text").ToList();

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

            var filterText = "Text";
            unitOfWork.AccountRepository.Add(new AccountEntity { Name = filterText });
            unitOfWork.AccountRepository.Add(new AccountEntity { Name = "testAccount"});
            unitOfWork.AccountRepository.Add(new AccountEntity { Name = "testAccount"});
            await unitOfWork.Commit();

            // Act
            var resultList = unitOfWork.AccountRepository.GetMany(x => x.Name == filterText).ToList();

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

            unitOfWork.AccountRepository.Add(new AccountEntity { Name = "testAccount"});
            unitOfWork.AccountRepository.Add(new AccountEntity { Name = "testAccount"});
            unitOfWork.AccountRepository.Add(new AccountEntity { Name = "testAccount"});
            await unitOfWork.Commit();

            // Act
            var result = await unitOfWork.AccountRepository.Get(x => x.Name == "text");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void Get_MatchedDataReturned()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var filterText = "Text";
            var testEntry = new AccountEntity { Name = filterText };
            unitOfWork.AccountRepository.Add(testEntry);
            unitOfWork.AccountRepository.Add(new AccountEntity { Name = "testAccount"});
            unitOfWork.AccountRepository.Add(new AccountEntity { Name = "testAccount"});
            await unitOfWork.Commit();

            // Act
            var result = await unitOfWork.AccountRepository.Get(x => x.Name == filterText);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testEntry.Id, result.Id);
        }
    }
}