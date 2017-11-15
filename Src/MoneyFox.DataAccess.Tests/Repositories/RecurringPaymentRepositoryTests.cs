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
            var recurringPaymentRepository = new RecurringPaymentRepository(ambientDbContextLocator);
            var accountRepository = new AccountRepository(ambientDbContextLocator);

            AccountEntity testAccount;

            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                testAccount = new AccountEntity {Name = "testAccount"};
                accountRepository.Add(testAccount);
                await dbContextScope.SaveChangesAsync();

            }

            var testEntry = new RecurringPaymentEntity
            {
                ChargedAccount = testAccount,
                Note = "Testtext"
            };

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                recurringPaymentRepository.Add(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            using (dbContextScopeFactory.CreateReadOnly())
            {
                var loadedEntry = await recurringPaymentRepository.GetById(testEntry.Id);
                Assert.Equal(testEntry.Note, loadedEntry.Note);
            }
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
            var recurringPaymentRepository = new RecurringPaymentRepository(ambientDbContextLocator);
            var accountRepository = new AccountRepository(ambientDbContextLocator);

            AccountEntity testAccount;
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                testAccount = new AccountEntity {Name = "testAccount"};
                accountRepository.Add(testAccount);
                await dbContextScope.SaveChangesAsync();
            }

            var testEntry = new RecurringPaymentEntity
            {
                ChargedAccount = testAccount,
                Note = "Testtext"
            };

            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                recurringPaymentRepository.Add(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {

                testEntry.Id = 0;
                recurringPaymentRepository.Add(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            using (dbContextScopeFactory.CreateReadOnly())
            {
                Assert.Equal(2, recurringPaymentRepository.GetAll().Count());
            }
        }

        [Fact]
        public async void Add_IdSet()
        {
            // Arrange
            var recurringPaymentRepository = new RecurringPaymentRepository(ambientDbContextLocator);
            var accountRepository = new AccountRepository(ambientDbContextLocator);

            AccountEntity testAccount;
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                testAccount = new AccountEntity { Name = "testAccount" };
                accountRepository.Add(testAccount);
                await dbContextScope.SaveChangesAsync();
            }

            var testEntry = new RecurringPaymentEntity
            {
                ChargedAccount = testAccount,
                Note = "Testtext"
            };

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                recurringPaymentRepository.Add(testEntry);
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
            var recurringPaymentRepository = new RecurringPaymentRepository(ambientDbContextLocator);
            var accountRepository = new AccountRepository(ambientDbContextLocator);

            AccountEntity testAccount;
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                testAccount = new AccountEntity { Name = "testAccount" };
                accountRepository.Add(testAccount);
                await dbContextScope.SaveChangesAsync();
            }

            var testEntry = new RecurringPaymentEntity {ChargedAccount = testAccount};

            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                recurringPaymentRepository.Add(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                recurringPaymentRepository.Delete(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            using (dbContextScopeFactory.CreateReadOnly())
            {
                Assert.Equal(0, recurringPaymentRepository.GetAll().Count());
            }
        }


        [Fact]
        public async void Delete_EntryMatchedFilterDeleted()
        {
            // Arrange
            var filterText = "Text";
            var recurringPaymentRepository = new RecurringPaymentRepository(ambientDbContextLocator);
            var accountRepository = new AccountRepository(ambientDbContextLocator);

            AccountEntity testAccount;
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                testAccount = new AccountEntity { Name = "testAccount" };
                accountRepository.Add(testAccount);
                await dbContextScope.SaveChangesAsync();
            }

            var testEntry1 = new RecurringPaymentEntity
            {
                ChargedAccount = testAccount,
                Note = filterText
            };

            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                var testEntry2 = new RecurringPaymentEntity {ChargedAccount = testAccount};
                recurringPaymentRepository.Add(testEntry1);
                recurringPaymentRepository.Add(testEntry2);
                await dbContextScope.SaveChangesAsync();
            }

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                recurringPaymentRepository.Delete(x => x.Note == filterText);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            using (dbContextScopeFactory.CreateReadOnly())
            {
                Assert.Equal(1, recurringPaymentRepository.GetAll().Count());
            }
        }

        [Fact]
        public async void Delete_EntryNotFound()
        {
            // Arrange
            var recurringPaymentRepository = new RecurringPaymentRepository(ambientDbContextLocator);
            var accountRepository = new AccountRepository(ambientDbContextLocator);

            AccountEntity testAccount;
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                testAccount = new AccountEntity { Name = "testAccount" };
                accountRepository.Add(testAccount);
                await dbContextScope.SaveChangesAsync();
            }

            var testEntry = new RecurringPaymentEntity {ChargedAccount = testAccount };

            // Act / Assert
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                recurringPaymentRepository.Delete(testEntry);
                await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await dbContextScope.SaveChangesAsync());
            }
        }

        [Fact]
        public async void Delete_RelatedPaymentSetNull()
        {
            // Arrange
            var recurringPaymentRepository = new RecurringPaymentRepository(ambientDbContextLocator);
            var paymentRepository = new PaymentRepository(ambientDbContextLocator);
            var accountRepository = new AccountRepository(ambientDbContextLocator);

            AccountEntity testAccount;
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                testAccount = new AccountEntity { Name = "testAccount" };
                accountRepository.Add(testAccount);
                await dbContextScope.SaveChangesAsync();
            }

            var recurringPaymentEntity = new RecurringPaymentEntity {ChargedAccount = testAccount };
            var payment = new PaymentEntity
            {
                ChargedAccount = testAccount,
                RecurringPayment = recurringPaymentEntity
            };

            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                paymentRepository.Add(payment);
                await dbContextScope.SaveChangesAsync();
            }

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                recurringPaymentRepository.Delete(recurringPaymentEntity);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            using (dbContextScopeFactory.CreateReadOnly())
            {
                Assert.Null(payment.RecurringPayment);
                Assert.Null(paymentRepository.GetById(payment.Id).Result.RecurringPayment);
            }
        }

        [Fact]
        public async void Get_MatchedDataReturned()
        {
            // Arrange
            var repository = new RecurringPaymentRepository(ambientDbContextLocator);
            var accountRepository = new AccountRepository(ambientDbContextLocator);

            AccountEntity testAccount;
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                testAccount = new AccountEntity { Name = "testAccount" };
                accountRepository.Add(testAccount);
                await dbContextScope.SaveChangesAsync();
            }

            var filterText = "Text";
            var testEntry = new RecurringPaymentEntity
            {
                ChargedAccount = testAccount,
                Note = filterText
            };
            RecurringPaymentEntity result;

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                repository.Add(testEntry);
                repository.Add(new RecurringPaymentEntity {ChargedAccount = testAccount });
                repository.Add(new RecurringPaymentEntity {ChargedAccount = testAccount });
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
            var recurringPaymentRepository = new RecurringPaymentRepository(ambientDbContextLocator);
            var accountRepository = new AccountRepository(ambientDbContextLocator);

            AccountEntity testAccount;
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                testAccount = new AccountEntity { Name = "testAccount" };
                accountRepository.Add(testAccount);
                await dbContextScope.SaveChangesAsync();
            }
            RecurringPaymentEntity result;

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                recurringPaymentRepository.Add(new RecurringPaymentEntity {ChargedAccount = testAccount });
                recurringPaymentRepository.Add(new RecurringPaymentEntity {ChargedAccount = testAccount });
                recurringPaymentRepository.Add(new RecurringPaymentEntity {ChargedAccount = testAccount });

                await dbContextScope.SaveChangesAsync();
                result = await recurringPaymentRepository.Get(x => x.Note == "text");
            }

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void GetAll_AllDataReturned()
        {
            // Arrange
            var recurringPaymentRepository = new RecurringPaymentRepository(ambientDbContextLocator);
            var accountRepository = new AccountRepository(ambientDbContextLocator);

            AccountEntity testAccount;
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                testAccount = new AccountEntity { Name = "testAccount" };
                accountRepository.Add(testAccount);
                await dbContextScope.SaveChangesAsync();
            }

            List<RecurringPaymentEntity> resultList;

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                recurringPaymentRepository.Add(new RecurringPaymentEntity {ChargedAccount = testAccount });
                recurringPaymentRepository.Add(new RecurringPaymentEntity {ChargedAccount = testAccount });
                recurringPaymentRepository.Add(new RecurringPaymentEntity {ChargedAccount = testAccount });

                await dbContextScope.SaveChangesAsync();
                resultList = recurringPaymentRepository.GetAll().ToList();
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
            using (dbContextScopeFactory.CreateReadOnly())
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
            var recurringPaymentRepository = new RecurringPaymentRepository(ambientDbContextLocator);
            var accountRepository = new AccountRepository(ambientDbContextLocator);

            AccountEntity testAccount;
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                testAccount = new AccountEntity { Name = "testAccount" };
                accountRepository.Add(testAccount);
                await dbContextScope.SaveChangesAsync();
            }

            var filterText = "Text";
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                recurringPaymentRepository.Add(new RecurringPaymentEntity
                {
                    ChargedAccount = testAccount,
                    Note = filterText
                });
                await dbContextScope.SaveChangesAsync();
            }
            List<RecurringPaymentEntity> resultList;

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                recurringPaymentRepository.Add(new RecurringPaymentEntity {ChargedAccount = testAccount });
                recurringPaymentRepository.Add(new RecurringPaymentEntity {ChargedAccount = testAccount });

                await dbContextScope.SaveChangesAsync();
                resultList = recurringPaymentRepository.GetMany(x => x.Note == filterText).ToList();
            }

            // Assert
            Assert.NotNull(resultList);
            Assert.Equal(1, resultList.Count);
        }

        [Fact]
        public async void GetMany_NothingMatched()
        {
            // Arrange
            var recurringPaymentRepository = new RecurringPaymentRepository(ambientDbContextLocator);
            var accountRepository = new AccountRepository(ambientDbContextLocator);

            AccountEntity testAccount;
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                testAccount = new AccountEntity { Name = "testAccount" };
                accountRepository.Add(testAccount);
                await dbContextScope.SaveChangesAsync();
            }
            List<RecurringPaymentEntity> resultList;

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                recurringPaymentRepository.Add(new RecurringPaymentEntity {ChargedAccount = testAccount });
                recurringPaymentRepository.Add(new RecurringPaymentEntity {ChargedAccount = testAccount });
                recurringPaymentRepository.Add(new RecurringPaymentEntity {ChargedAccount = testAccount });
                await dbContextScope.SaveChangesAsync();
                resultList = recurringPaymentRepository.GetMany(x => x.Note == "text").ToList();
            }

            // Assert
            Assert.NotNull(resultList);
            Assert.False(resultList.Any());
        }

        [Fact]
        public async void Update_EntryUpdated()
        {
            // Arrange
            var recurringPaymentRepository = new RecurringPaymentRepository(ambientDbContextLocator);
            var accountRepository = new AccountRepository(ambientDbContextLocator);

            AccountEntity testAccount;
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                testAccount = new AccountEntity { Name = "testAccount" };
                accountRepository.Add(testAccount);
                await dbContextScope.SaveChangesAsync();
            }

            var newValue = "newText";
            var testEntry = new RecurringPaymentEntity
            {
                ChargedAccount = testAccount,
                Note = "Testtext"
            };

            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                recurringPaymentRepository.Add(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                testEntry.Note = newValue;
                recurringPaymentRepository.Update(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            using (dbContextScopeFactory.CreateReadOnly())
            {
                var loadedEntry = await recurringPaymentRepository.GetById(testEntry.Id);
                Assert.Equal(newValue, loadedEntry.Note);
            }
        }

        [Fact]
        public async void Update_IdUnchanged()
        {
            // Arrange
            var recurringPaymentRepository = new RecurringPaymentRepository(ambientDbContextLocator);
            var accountRepository = new AccountRepository(ambientDbContextLocator);

            AccountEntity testAccount;
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                testAccount = new AccountEntity { Name = "testAccount" };
                accountRepository.Add(testAccount);
                await dbContextScope.SaveChangesAsync();
            }

            var testEntry = new RecurringPaymentEntity
            {
                ChargedAccount = testAccount,
                Note = "Testtext"
            };

            int idBeforeUpdate;

            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                recurringPaymentRepository.Add(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                idBeforeUpdate = testEntry.Id;
                recurringPaymentRepository.Update(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            Assert.Equal(idBeforeUpdate, testEntry.Id);
        }

        [Fact]
        public async void Update_NoNewEntryAdded()
        {
            // Arrange
            var recurringPaymentRepository = new RecurringPaymentRepository(ambientDbContextLocator);
            var accountRepository = new AccountRepository(ambientDbContextLocator);

            AccountEntity testAccount;
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                testAccount = new AccountEntity { Name = "testAccount" };
                accountRepository.Add(testAccount);
                await dbContextScope.SaveChangesAsync();
            }

            var testEntry = new RecurringPaymentEntity
            {
                ChargedAccount = testAccount,
                Note = "Testtext"
            };

            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                recurringPaymentRepository.Add(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                recurringPaymentRepository.Update(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            using (dbContextScopeFactory.CreateReadOnly())
            {
                Assert.Equal(1, recurringPaymentRepository.GetAll().Count());
            }
        }
    }
}