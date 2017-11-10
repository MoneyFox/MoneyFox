using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EntityFramework.DbContextScope;
using Microsoft.EntityFrameworkCore;
using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.Repositories;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Constants;
using Xunit;

namespace MoneyFox.DataAccess.Tests.Repositories
{
    public class PaymentRepositoryTests : IDisposable
    {
        private DbContextScopeFactory dbContextScopeFactory;
        private AmbientDbContextLocator ambientDbContextLocator;

        /// <summary>
        ///     Setup Logic who is executed before every test
        /// </summary>
        public PaymentRepositoryTests()
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
            var paymentRepository = new PaymentRepository(ambientDbContextLocator);
            var accountRepository = new AccountRepository(ambientDbContextLocator);

            AccountEntity account;
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                account = new AccountEntity {Name = "testAccount"};
                accountRepository.Add(account);
                await dbContextScope.SaveChangesAsync();
            }

            var testEntry = new PaymentEntity
            {
                ChargedAccount = account,
                Note = "Testtext"
            };

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                paymentRepository.Add(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            using (dbContextScopeFactory.CreateReadOnly())
            {
                var loadedEntry = await paymentRepository.GetById(testEntry.Id);
                Assert.Equal(testEntry.Note, loadedEntry.Note);
            }
        }

        [Fact]
        public async void Add_AddMultipleEntries()
        {
            // Arrange
            var paymentRepository = new PaymentRepository(ambientDbContextLocator);
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
                paymentRepository.Add(new PaymentEntity {ChargedAccount = account });
                paymentRepository.Add(new PaymentEntity {ChargedAccount = account });
                paymentRepository.Add(new PaymentEntity {ChargedAccount = account });
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            using (dbContextScopeFactory.CreateReadOnly())
            {
                Assert.Equal(3, paymentRepository.GetAll().Count());
            }
        }

        [Fact]
        public async void Add_AddNewEntryOnEveryCall()
        {
            // Arrange
            var paymentRepository = new PaymentRepository(ambientDbContextLocator);
            var accountRepository = new AccountRepository(ambientDbContextLocator);

            var account = new AccountEntity {Name = "testAccount"};
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                accountRepository.Add(account);
                await dbContextScope.SaveChangesAsync();
            }

            var testEntry = new PaymentEntity
            {
                ChargedAccount = account,
                Note = "Testtext"
            };

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                paymentRepository.Add(testEntry);
                await dbContextScope.SaveChangesAsync();
            }
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                testEntry.Id = 0;
                paymentRepository.Add(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            using (dbContextScopeFactory.CreateReadOnly())
            {
                Assert.Equal(2, paymentRepository.GetAll().Count());
            }
        }

        [Fact]
        public async void Add_IdSet()
        {
            // Arrange
            var paymentRepository = new PaymentRepository(ambientDbContextLocator);
            var accountRepository = new AccountRepository(ambientDbContextLocator);

            var account = new AccountEntity { Name = "testAccount" };
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                accountRepository.Add(account);
                await dbContextScope.SaveChangesAsync();
            }

            var testEntry = new PaymentEntity
            {
                ChargedAccount = account,
                Note = "Testtext"
            };

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                paymentRepository.Add(testEntry);
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
            var paymentRepository = new PaymentRepository(ambientDbContextLocator);
            var accountRepository = new AccountRepository(ambientDbContextLocator);

            var testAccount = new AccountEntity { Name = "testAccount" };
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                accountRepository.Add(testAccount);
                await dbContextScope.SaveChangesAsync();
            }

            var testEntry = new PaymentEntity {ChargedAccount = testAccount };

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                paymentRepository.Add(testEntry);
                await dbContextScope.SaveChangesAsync();
            }
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                paymentRepository.Delete(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            using (dbContextScopeFactory.CreateReadOnly())
            {
                Assert.Equal(0, paymentRepository.GetAll().Count());
            }
        }

        [Fact]
        public async void Delete_EntryMatchedFilterDeleted()
        {
            // Arrange
            var filterText = "Text";
            var paymentRepository = new PaymentRepository(ambientDbContextLocator);
            var accountRepository = new AccountRepository(ambientDbContextLocator);

            var testAccount = new AccountEntity { Name = "testAccount" };
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                accountRepository.Add(testAccount);
                await dbContextScope.SaveChangesAsync();
            }

            var testEntry1 = new PaymentEntity
            {
                ChargedAccount = testAccount,
                Note = filterText
            };
            var testEntry2 = new PaymentEntity {ChargedAccount = testAccount};
            
            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                paymentRepository.Add(testEntry1);
                paymentRepository.Add(testEntry2);
                await dbContextScope.SaveChangesAsync();
            }
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                paymentRepository.Delete(x => x.Note == filterText);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            using (dbContextScopeFactory.CreateReadOnly())
            {
                Assert.Equal(1, paymentRepository.GetAll().Count());
            }
        }

        [Fact]
        public async void Delete_EntryNotFound()
        {
            // Arrange
            var paymentRepository = new PaymentRepository(ambientDbContextLocator);
            var accountRepository = new AccountRepository(ambientDbContextLocator);

            var testAccount = new AccountEntity { Name = "testAccount" };
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                accountRepository.Add(testAccount);
                await dbContextScope.SaveChangesAsync();
            }

            var testEntry = new PaymentEntity {ChargedAccount = testAccount};

            // Act / Assert
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                paymentRepository.Delete(testEntry);
                await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await dbContextScope.SaveChangesAsync());
            }
        }

        [Fact]
        public async void Get_MatchedDataReturned()
        {
            // Arrange
            var paymentRepository = new PaymentRepository(ambientDbContextLocator);
            var accountRepository = new AccountRepository(ambientDbContextLocator);

            var testAccount = new AccountEntity { Name = "testAccount" };
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                accountRepository.Add(testAccount);
                await dbContextScope.SaveChangesAsync();
            }

            var filterText = "Text";
            var testEntry = new PaymentEntity
            {
                ChargedAccount = testAccount,
                Note = filterText
            };

            PaymentEntity result;

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                paymentRepository.Add(testEntry);
                paymentRepository.Add(new PaymentEntity {ChargedAccount = testAccount });
                paymentRepository.Add(new PaymentEntity {ChargedAccount = testAccount });
                await dbContextScope.SaveChangesAsync();

                result = await paymentRepository.Get(x => x.Note == filterText);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testEntry.Id, result.Id);
        }

        [Fact]
        public async void Get_NothingMatched()
        {
            // Arrange
            var paymentRepository = new PaymentRepository(ambientDbContextLocator);
            var accountRepository = new AccountRepository(ambientDbContextLocator);

            var testAccount = new AccountEntity { Name = "testAccount" };
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                accountRepository.Add(testAccount);
                await dbContextScope.SaveChangesAsync();
            }

            PaymentEntity result;

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                paymentRepository.Add(new PaymentEntity {ChargedAccount = testAccount});
                paymentRepository.Add(new PaymentEntity {ChargedAccount = testAccount});
                paymentRepository.Add(new PaymentEntity {ChargedAccount = testAccount});
                await dbContextScope.SaveChangesAsync();
                result = await paymentRepository.Get(x => x.Note == "text");
            }

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void GetAll_AllDataReturned()
        {
            // Arrange
            var paymentRepository = new PaymentRepository(ambientDbContextLocator);
            var accountRepository = new AccountRepository(ambientDbContextLocator);

            var testAccount = new AccountEntity { Name = "testAccount" };
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                accountRepository.Add(testAccount);
                await dbContextScope.SaveChangesAsync();
            }

            List<PaymentEntity> resultList;

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                paymentRepository.Add(new PaymentEntity {ChargedAccount = testAccount});
                paymentRepository.Add(new PaymentEntity {ChargedAccount = testAccount});
                paymentRepository.Add(new PaymentEntity {ChargedAccount = testAccount});
                await dbContextScope.SaveChangesAsync();
                resultList = paymentRepository.GetAll().ToList();
            }
            // Assert
            Assert.NotNull(resultList);
            Assert.Equal(3, resultList.Count);
        }

        [Fact]
        public void GetAll_NoData()
        {
            // Arrange
            var repository = new PaymentRepository(ambientDbContextLocator);
            List<PaymentEntity> resultList;

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
            var paymentRepository = new PaymentRepository(ambientDbContextLocator);
            var accountRepository = new AccountRepository(ambientDbContextLocator);

            var testAccount = new AccountEntity { Name = "testAccount" };
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                accountRepository.Add(testAccount);
                await dbContextScope.SaveChangesAsync();
            }

            var filterText = "Text";
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                paymentRepository.Add(new PaymentEntity
                {
                    ChargedAccount = testAccount,
                    Note = filterText
                });
                await dbContextScope.SaveChangesAsync();
            }
            List<PaymentEntity> resultList;

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                paymentRepository.Add(new PaymentEntity {ChargedAccount = testAccount});
                paymentRepository.Add(new PaymentEntity {ChargedAccount = testAccount});
                await dbContextScope.SaveChangesAsync();

                resultList = paymentRepository.GetMany(x => x.Note == filterText).ToList();
            }

            // Assert
            Assert.NotNull(resultList);
            Assert.Equal(1, resultList.Count);
        }

        [Fact]
        public async void GetMany_NothingMatched()
        {
            // Arrange
            var paymentRepository = new PaymentRepository(ambientDbContextLocator);
            var accountRepository = new AccountRepository(ambientDbContextLocator);

            var testAccount = new AccountEntity { Name = "testAccount" };
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                accountRepository.Add(testAccount);
                await dbContextScope.SaveChangesAsync();
            }
            List<PaymentEntity> resultList;

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                paymentRepository.Add(new PaymentEntity {ChargedAccount = testAccount});
                paymentRepository.Add(new PaymentEntity {ChargedAccount = testAccount});
                paymentRepository.Add(new PaymentEntity {ChargedAccount = testAccount});
                await dbContextScope.SaveChangesAsync();
                resultList = paymentRepository.GetMany(x => x.Note == "text").ToList();
            }

            // Assert
            Assert.NotNull(resultList);
            Assert.False(resultList.Any());
        }

        [Fact]
        public async void Update_EntryUpdated()
        {
            // Arrange
            var paymentRepository = new PaymentRepository(ambientDbContextLocator);
            var accountRepository = new AccountRepository(ambientDbContextLocator);

            var testAccount = new AccountEntity { Name = "testAccount" };
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                accountRepository.Add(testAccount);
                await dbContextScope.SaveChangesAsync();
            }

            var newValue = "newText";
            var testEntry = new PaymentEntity
            {
                ChargedAccount = testAccount,
                Note = "Testtext"
            };

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                paymentRepository.Add(testEntry);
                await dbContextScope.SaveChangesAsync();
            }
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                testEntry.Note = newValue;
                paymentRepository.Update(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            using (dbContextScopeFactory.CreateReadOnly())
            {
                var loadedEntry = await paymentRepository.GetById(testEntry.Id);
                Assert.Equal(newValue, loadedEntry.Note);
            }
        }

        [Fact]
        public async void Update_IdUnchanged()
        {
            // Arrange
            var paymentRepository = new PaymentRepository(ambientDbContextLocator);
            var accountRepository = new AccountRepository(ambientDbContextLocator);

            var testAccount = new AccountEntity { Name = "testAccount" };
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                accountRepository.Add(testAccount);
                await dbContextScope.SaveChangesAsync();
            }

            var testEntry = new PaymentEntity
            {
                ChargedAccount = testAccount,
                Note = "Testtext"
            };
            int idBeforeUpdate;

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                paymentRepository.Add(testEntry);
                await dbContextScope.SaveChangesAsync();
            }
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                idBeforeUpdate = testEntry.Id;
                paymentRepository.Update(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            Assert.Equal(idBeforeUpdate, testEntry.Id);
        }

        [Fact]
        public async void Update_NoNewEntryAdded()
        {
            // Arrange
            var paymentRepository = new PaymentRepository(ambientDbContextLocator);
            var accountRepository = new AccountRepository(ambientDbContextLocator);

            var testAccount = new AccountEntity { Name = "testAccount" };
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                accountRepository.Add(testAccount);
                await dbContextScope.SaveChangesAsync();
            }

            var testEntry = new PaymentEntity
            {
                ChargedAccount = testAccount,
                Note = "Testtext"
            };

            // Act
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                paymentRepository.Add(testEntry);
                await dbContextScope.SaveChangesAsync();
            }
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                paymentRepository.Update(testEntry);
                await dbContextScope.SaveChangesAsync();
            }

            // Assert
            using (dbContextScopeFactory.CreateReadOnly())
            {
                Assert.Equal(1, paymentRepository.GetAll().Count());
            }
        }
    }
}