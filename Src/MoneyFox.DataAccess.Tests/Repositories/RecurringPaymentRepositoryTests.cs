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
    public class RecurringRecurringPaymentRepositoryTests : IDisposable
    {
        private DbContextScopeFactory dbContextScopeFactory;
        private AmbientDbContextLocator ambientDbContextLocator;

        /// <summary>
        ///     Setup Logic who is executed before every test
        /// </summary>
        public RecurringRecurringPaymentRepositoryTests()
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
            var repository = new RecurringPaymentRepository(ambientDbContextLocator);

            var testEntry = new RecurringPaymentEntity
            {
                ChargedAccount = new AccountEntity {Name = "testAccount"},
                Note = "Testtext"
            };

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Add(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            var loadedEntry = await repository.GetById(testEntry.Id);
            Assert.Equal(testEntry.Note, loadedEntry.Note);
        }

        [Fact]
        public async void Add_AddMultipleEntries()
        {
            // Arrange
            var recurringPaymentRepository = new RecurringPaymentRepository(ambientDbContextLocator);
            var accountRepository = new AccountRepository(ambientDbContextLocator);

            AccountEntity account;
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                account = new AccountEntity {Name = "testAccount"};
                accountRepository.Add(account);
                await dbContextScope.SaveChangesAsync();
            }

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                recurringPaymentRepository.Add(new RecurringPaymentEntity {ChargedAccount = account});
                recurringPaymentRepository.Add(new RecurringPaymentEntity {ChargedAccount = account });
                recurringPaymentRepository.Add(new RecurringPaymentEntity {ChargedAccount = account });
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            using (dbContextScopeFactory.CreateReadOnly())
            {
                Assert.Equal(3, recurringPaymentRepository.GetAll().Count());
            }
        }

        [Fact]
        public async void Add_AddNewEntryOnEveryCall()
        {
            // Arrange
            var repository = new RecurringPaymentRepository(ambientDbContextLocator);

            var testEntry = new RecurringPaymentEntity
            {
                ChargedAccount = new AccountEntity {Name = "testAccount"},
                Note = "Testtext"
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
            var repository = new RecurringPaymentRepository(ambientDbContextLocator);

            var testEntry = new RecurringPaymentEntity
            {
                ChargedAccount = new AccountEntity {Name = "testAccount"},
                Note = "Testtext"
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
        public async void Delete_EntryDeleted()
        {
            // Arrange
            var repository = new RecurringPaymentRepository(ambientDbContextLocator);
            var testEntry = new RecurringPaymentEntity {ChargedAccount = new AccountEntity {Name = "testAccount"}};


            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
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
            var filterText = "Text";
            var repository = new RecurringPaymentRepository(ambientDbContextLocator);
            var testEntry1 = new RecurringPaymentEntity
            {
                ChargedAccount = new AccountEntity {Name = "testAccount"},
                Note = filterText
            };
            var testEntry2 = new RecurringPaymentEntity {ChargedAccount = new AccountEntity {Name = "testAccount"}};


            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Add(testEntry1);
                repository.Add(testEntry2);
                await dbContextScope.SaveChangesAsync();

                repository.Delete(x => x.Note == filterText);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            Assert.Equal(1, repository.GetAll().Count());
        }

        [Fact]
        public async void Delete_EntryNotFound()
        {
            // Arrange
            var repository = new RecurringPaymentRepository(ambientDbContextLocator);
            var testEntry = new RecurringPaymentEntity {ChargedAccount = new AccountEntity {Name = "testAccount"}};

            // Act / Assert
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Delete(testEntry);
                await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await dbContextScope.SaveChangesAsync());
            }
        }

        [Fact]
        public async void Delete_RelatedPaymentSetNull()
        {
            // Arrange
            var recurringPaymentRepository = new RecurringPaymentRepository(ambientDbContextLocator);
            var paymentRepository = new PaymentRepository(ambientDbContextLocator);

            var recurringPaymentEntity = new RecurringPaymentEntity {ChargedAccount = new AccountEntity {Name = "testAccount"}};
            var payment = new PaymentEntity
            {
                ChargedAccount = new AccountEntity {Name = "testAccount"},
                RecurringPayment = recurringPaymentEntity
            };

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                paymentRepository.Add(payment);
                await dbContextScope.SaveChangesAsync();
                recurringPaymentRepository.Delete(recurringPaymentEntity);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            Assert.Null(payment.RecurringPayment);
            Assert.Null(paymentRepository.GetById(payment.Id).Result.RecurringPayment);
        }

        [Fact]
        public async void Get_MatchedDataReturned()
        {
            // Arrange
            var repository = new RecurringPaymentRepository(ambientDbContextLocator);
            var filterText = "Text";
            var testEntry = new RecurringPaymentEntity
            {
                ChargedAccount = new AccountEntity {Name = "testAccount"},
                Note = filterText
            };
            RecurringPaymentEntity result;

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Add(testEntry);
                repository.Add(new RecurringPaymentEntity {ChargedAccount = new AccountEntity {Name = "testAccount"}});
                repository.Add(new RecurringPaymentEntity {ChargedAccount = new AccountEntity {Name = "testAccount"}});
                await dbContextScope.SaveChangesAsync();
                result = await repository.Get(x => x.Note == filterText);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testEntry.Id, result.Id);
        }

        [Fact]
        public async void Get_NothingMatched()
        {
            // Arrange
            var repository = new RecurringPaymentRepository(ambientDbContextLocator);
            RecurringPaymentEntity result;

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Add(new RecurringPaymentEntity {ChargedAccount = new AccountEntity {Name = "testAccount"}});
                repository.Add(new RecurringPaymentEntity {ChargedAccount = new AccountEntity {Name = "testAccount"}});
                repository.Add(new RecurringPaymentEntity {ChargedAccount = new AccountEntity {Name = "testAccount"}});

                await dbContextScope.SaveChangesAsync();
                result = await repository.Get(x => x.Note == "text");
            }

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void GetAll_AllDataReturned()
        {
            // Arrange
            var repository = new RecurringPaymentRepository(ambientDbContextLocator);

            List<RecurringPaymentEntity> resultList;

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Add(new RecurringPaymentEntity {ChargedAccount = new AccountEntity {Name = "testAccount"}});
                repository.Add(new RecurringPaymentEntity {ChargedAccount = new AccountEntity {Name = "testAccount"}});
                repository.Add(new RecurringPaymentEntity {ChargedAccount = new AccountEntity {Name = "testAccount"}});

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
            var repository = new RecurringPaymentRepository(ambientDbContextLocator);
            List<RecurringPaymentEntity> resultList;

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
            var repository = new RecurringPaymentRepository(ambientDbContextLocator);
            var filterText = "Text";
            repository.Add(new RecurringPaymentEntity
            {
                ChargedAccount = new AccountEntity {Name = "testAccount"},
                Note = filterText
            });
            List<RecurringPaymentEntity> resultList;

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Add(new RecurringPaymentEntity {ChargedAccount = new AccountEntity {Name = "testAccount"}});
                repository.Add(new RecurringPaymentEntity {ChargedAccount = new AccountEntity {Name = "testAccount"}});

                await dbContextScope.SaveChangesAsync();
                resultList = repository.GetMany(x => x.Note == filterText).ToList();
            }

            // Assert
            Assert.NotNull(resultList);
            Assert.Equal(1, resultList.Count);
        }

        [Fact]
        public async void GetMany_NothingMatched()
        {
            // Arrange
            var repository = new RecurringPaymentRepository(ambientDbContextLocator);
            List<RecurringPaymentEntity> resultList;

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Add(new RecurringPaymentEntity {ChargedAccount = new AccountEntity {Name = "testAccount"}});
                repository.Add(new RecurringPaymentEntity {ChargedAccount = new AccountEntity {Name = "testAccount"}});
                repository.Add(new RecurringPaymentEntity {ChargedAccount = new AccountEntity {Name = "testAccount"}});
                await dbContextScope.SaveChangesAsync();
                resultList = repository.GetMany(x => x.Note == "text").ToList();
            }

            // Assert
            Assert.NotNull(resultList);
            Assert.False(resultList.Any());
        }

        [Fact]
        public async void Update_EntryUpdated()
        {
            // Arrange
            var repository = new RecurringPaymentRepository(ambientDbContextLocator);

            var newValue = "newText";
            var testEntry = new RecurringPaymentEntity
            {
                ChargedAccount = new AccountEntity {Name = "testAccount"},
                Note = "Testtext"
            };

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Add(testEntry);
                await dbContextScope.SaveChangesAsync();

                testEntry.Note = newValue;
                repository.Update(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            var loadedEntry = await repository.GetById(testEntry.Id);
            Assert.Equal(newValue, loadedEntry.Note);
        }

        [Fact]
        public async void Update_IdUnchanged()
        {
            // Arrange
            var repository = new RecurringPaymentRepository(ambientDbContextLocator);

            var testEntry = new RecurringPaymentEntity
            {
                ChargedAccount = new AccountEntity {Name = "testAccount"},
                Note = "Testtext"
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
            var repository = new RecurringPaymentRepository(ambientDbContextLocator);

            var testEntry = new RecurringPaymentEntity
            {
                ChargedAccount = new AccountEntity {Name = "testAccount"},
                Note = "Testtext"
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