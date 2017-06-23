using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.Infrastructure;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Constants;
using Xunit;

namespace MoneyFox.DataAccess.Tests.Repositories
{
    public class PaymentRepositoryTests : IDisposable
    {
        /// <summary>
        ///     Setup Logic who is executed before every test
        /// </summary>
        public PaymentRepositoryTests()
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
        public async void Add_AddedAndRead()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var testEntry = new PaymentEntity
            {
                ChargedAccount = new AccountEntity { Name = "testAccount"},
                Note = "Testtext"
            };

            // Act
            unitOfWork.PaymentRepository.Add(testEntry);
            await unitOfWork.Commit();

            // Assert
            var loadedEntry = await unitOfWork.PaymentRepository.GetById(testEntry.Id);
            Assert.Equal(testEntry.Note, loadedEntry.Note);
        }

        [Fact]
        public async void Add_AddMultipleEntries()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            // Act
            unitOfWork.PaymentRepository.Add(new PaymentEntity {ChargedAccount = new AccountEntity { Name = "testAccount" }});
            unitOfWork.PaymentRepository.Add(new PaymentEntity { ChargedAccount = new AccountEntity { Name = "testAccount" } });
            unitOfWork.PaymentRepository.Add(new PaymentEntity { ChargedAccount = new AccountEntity { Name = "testAccount" } });
            await unitOfWork.Commit();

            // Assert
            Assert.Equal(3, unitOfWork.PaymentRepository.GetAll().Count());
        }

        [Fact]
        public async void Add_AddNewEntryOnEveryCall()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var testEntry = new PaymentEntity
            {
                ChargedAccount = new AccountEntity { Name = "testAccount"},
                Note = "Testtext"
            };

            // Act
            unitOfWork.PaymentRepository.Add(testEntry);
            await unitOfWork.Commit();
            testEntry.Id = 0;
            unitOfWork.PaymentRepository.Add(testEntry);
            await unitOfWork.Commit();

            // Assert
            Assert.Equal(2, unitOfWork.PaymentRepository.GetAll().Count());
        }

        [Fact]
        public async void Add_IdSet()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var testEntry = new PaymentEntity
            {
                ChargedAccount = new AccountEntity { Name = "testAccount"},
                Note = "Testtext"
            };

            // Act
            unitOfWork.PaymentRepository.Add(testEntry);
            await unitOfWork.Commit();

            // Assert
            Assert.NotNull(testEntry.Id);
            Assert.NotEqual(0, testEntry.Id);
        }

        [Fact]
        public async void Add_WithoutAccount()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var testEntry = new PaymentEntity
            {
                Note = "Testtext"
            };

            // Act / Assert
            unitOfWork.PaymentRepository.Add(testEntry);
            await Assert.ThrowsAsync<DbUpdateException>(async () => await unitOfWork.Commit());
        }

        [Fact]
        public async void Add_WithRecurringPayment()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var testAccount = new AccountEntity {Name = "testAccount"};
            var testEntry = new PaymentEntity
            {
                ChargedAccount = testAccount,
                RecurringPayment = new RecurringPaymentEntity
                {
                    ChargedAccount = testAccount,
                    Recurrence = PaymentRecurrence.Bimonthly,
                    IsEndless = true
                },
                IsRecurring = true,
                Note = "Testtext"
            };

            // Act
            unitOfWork.PaymentRepository.Add(testEntry);
            await unitOfWork.Commit();

            // Assert
            Assert.NotNull(testEntry.Id);
            Assert.NotEqual(0, testEntry.Id);
            Assert.NotEqual(0, testEntry.RecurringPayment.Id);
            Assert.NotEqual(0, testEntry.RecurringPaymentId);
        }

        [Fact]
        public async void Update_EntryUpdated()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var newValue = "newText";
            var testEntry = new PaymentEntity
            {
                ChargedAccount = new AccountEntity { Name = "testAccount"},
                Note = "Testtext"
            };

            // Act
            unitOfWork.PaymentRepository.Add(testEntry);
            await unitOfWork.Commit();

            testEntry.Note = newValue;
            unitOfWork.PaymentRepository.Update(testEntry);
            await unitOfWork.Commit();

            // Assert
            var loadedEntry = await unitOfWork.PaymentRepository.GetById(testEntry.Id);
            Assert.Equal(newValue, loadedEntry.Note);
        }

        [Fact]
        public async void Update_IdUnchanged()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var testEntry = new PaymentEntity
            {
                ChargedAccount = new AccountEntity { Name = "testAccount"},
                Note = "Testtext"
            };

            // Act
            unitOfWork.PaymentRepository.Add(testEntry);
            await unitOfWork.Commit();

            var idBeforeUpdate = testEntry.Id;
            unitOfWork.PaymentRepository.Update(testEntry);
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

            var testEntry = new PaymentEntity
            {
                ChargedAccount = new AccountEntity { Name = "testAccount"},
                Note = "Testtext"
            };

            // Act
            unitOfWork.PaymentRepository.Add(testEntry);
            await unitOfWork.Commit();

            unitOfWork.PaymentRepository.Update(testEntry);
            await unitOfWork.Commit();

            // Assert
            Assert.Equal(1, unitOfWork.PaymentRepository.GetAll().Count());
        }

        [Fact]
        public async void Delete_EntryDeleted()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var testEntry = new PaymentEntity { ChargedAccount = new AccountEntity { Name = "testAccount"} };
            unitOfWork.PaymentRepository.Add(testEntry);
            await unitOfWork.Commit();

            // Act
            unitOfWork.PaymentRepository.Delete(testEntry);
            await unitOfWork.Commit();

            // Assert
            Assert.Equal(0, unitOfWork.PaymentRepository.GetAll().Count());
        }

        [Fact]
        public async void Delete_EntryNotFound()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var testEntry = new PaymentEntity { ChargedAccount = new AccountEntity { Name = "testAccount"} };

            // Act
            unitOfWork.PaymentRepository.Delete(testEntry);

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
            var testEntry1 = new PaymentEntity
            {
                ChargedAccount = new AccountEntity { Name = "testAccount"},
                Note = filterText
            };
            var testEntry2 = new PaymentEntity { ChargedAccount = new AccountEntity { Name = "testAccount"} };
            unitOfWork.PaymentRepository.Add(testEntry1);
            unitOfWork.PaymentRepository.Add(testEntry2);
            await unitOfWork.Commit();

            // Act
            unitOfWork.PaymentRepository.Delete(x => x.Note == filterText);
            await unitOfWork.Commit();

            // Assert
            Assert.Equal(1, unitOfWork.PaymentRepository.GetAll().Count());
        }

        [Fact]
        public void GetAll_NoData()
        {
            // Arrange
            var unitOfWork= new UnitOfWork(new DbFactory());

            // Act
            var emptyList = unitOfWork.PaymentRepository.GetAll().ToList();

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

            unitOfWork.PaymentRepository.Add(new PaymentEntity { ChargedAccount = new AccountEntity { Name = "testAccount"} });
            unitOfWork.PaymentRepository.Add(new PaymentEntity { ChargedAccount = new AccountEntity { Name = "testAccount"} });
            unitOfWork.PaymentRepository.Add(new PaymentEntity { ChargedAccount = new AccountEntity { Name = "testAccount"} });
            await unitOfWork.Commit();

            // Act
            var resultList = unitOfWork.PaymentRepository.GetAll().ToList();

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

            unitOfWork.PaymentRepository.Add(new PaymentEntity { ChargedAccount = new AccountEntity { Name = "testAccount"} });
            unitOfWork.PaymentRepository.Add(new PaymentEntity { ChargedAccount = new AccountEntity { Name = "testAccount"} });
            unitOfWork.PaymentRepository.Add(new PaymentEntity { ChargedAccount = new AccountEntity { Name = "testAccount"} });
            await unitOfWork.Commit();

            // Act
            var resultList = unitOfWork.PaymentRepository.GetMany(x => x.Note == "text").ToList();

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
            unitOfWork.PaymentRepository.Add(new PaymentEntity
            {
                ChargedAccount = new AccountEntity { Name = "testAccount"},
                Note = filterText
            });
            unitOfWork.PaymentRepository.Add(new PaymentEntity { ChargedAccount = new AccountEntity { Name = "testAccount"} });
            unitOfWork.PaymentRepository.Add(new PaymentEntity { ChargedAccount = new AccountEntity { Name = "testAccount"} });
            await unitOfWork.Commit();

            // Act
            var resultList = unitOfWork.PaymentRepository.GetMany(x => x.Note == filterText).ToList();

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

            unitOfWork.PaymentRepository.Add(new PaymentEntity { ChargedAccount = new AccountEntity { Name = "testAccount"} });
            unitOfWork.PaymentRepository.Add(new PaymentEntity { ChargedAccount = new AccountEntity { Name = "testAccount"} });
            unitOfWork.PaymentRepository.Add(new PaymentEntity { ChargedAccount = new AccountEntity { Name = "testAccount"} });
            await unitOfWork.Commit();

            // Act
            var result = await unitOfWork.PaymentRepository.Get(x => x.Note == "text");

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
            var testEntry = new PaymentEntity
            {
                ChargedAccount = new AccountEntity { Name = "testAccount"},
                Note = filterText
            };
            unitOfWork.PaymentRepository.Add(testEntry);
            unitOfWork.PaymentRepository.Add(new PaymentEntity { ChargedAccount = new AccountEntity { Name = "testAccount"} });
            unitOfWork.PaymentRepository.Add(new PaymentEntity { ChargedAccount = new AccountEntity { Name = "testAccount"} });
            await unitOfWork.Commit();

            // Act
            var result = await unitOfWork.PaymentRepository.Get(x => x.Note == filterText);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testEntry.Id, result.Id);
        }
    }
}