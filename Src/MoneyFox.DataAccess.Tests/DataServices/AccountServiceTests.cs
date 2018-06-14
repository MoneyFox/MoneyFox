using System;
using System.IO;
using System.Linq;
using EntityFramework.DbContextScope;
using Microsoft.EntityFrameworkCore;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Foundation.Constants;
using Xunit;

namespace MoneyFox.DataAccess.Tests.DataServices
{
    public class AccountServiceTests
    {
        private readonly DbContextScopeFactory dbContextScopeFactory;
        private readonly AmbientDbContextLocator ambientDbContextLocator;

        /// <summary>
        ///     Setup Logic who is executed before every test
        /// </summary>
        public AccountServiceTests()
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
        public async void SaveAccount_AccountSavedAndLoaded()
        {
            // Arrange
            var accountService = new AccountService(ambientDbContextLocator, dbContextScopeFactory);

            var testEntry = new AccountEntity
            {
                Name = "Testtext"
            };

            // Act
            await accountService.SaveAccount(new Account(testEntry));

            // Assert
            var loadedEntry = await accountService.GetById(testEntry.Id);
            Assert.Equal(testEntry.Name, loadedEntry.Data.Name);
        }

        [Fact]
        public async void SaveAccount_AccountSavedIdSet()
        {
            // Arrange
            var accountService = new AccountService(ambientDbContextLocator, dbContextScopeFactory);

            var testEntry = new AccountEntity
            {
                Name = "Testtext"
            };

            // Act
            await accountService.SaveAccount(new Account(testEntry));

            // Assert
            Assert.NotEqual(0, testEntry.Id);
        }

        [Fact]
        public async void SaveAccount_MultipleAccountSavedAndLoaded()
        {
            // Arrange
            var accountService = new AccountService(ambientDbContextLocator, dbContextScopeFactory);

            var testEntry1 = new AccountEntity
            {
                Name = "Testtext"
            };
            var testEntry2 = new AccountEntity
            {
                Name = "aasdf"
            };

            // Act
            await accountService.SaveAccount(new Account(testEntry1));
            await accountService.SaveAccount(new Account(testEntry2));

            // Assert
            Assert.Equal(testEntry1.Name, (await accountService.GetById(testEntry1.Id)).Data.Name);
            Assert.Equal(testEntry2.Name, (await accountService.GetById(testEntry2.Id)).Data.Name);
        }

        [Fact]
        public async void SaveAccount_AccountUpdatedCorrectly()
        {
            // Arrange
            const string udpatedString = "new Account Name";
            var accountService = new AccountService(ambientDbContextLocator, dbContextScopeFactory);

            var testEntry = new AccountEntity
            {
                Name = "Testtext"
            };

            await accountService.SaveAccount(new Account(testEntry));
            testEntry.Name = udpatedString;

            // Act
            await accountService.SaveAccount(new Account(testEntry));

            // Assert
            var loadedEntry = await accountService.GetById(testEntry.Id);
            Assert.Equal(udpatedString, loadedEntry.Data.Name);
        }
        
        [Fact]
        public async void Add_NewEntryWithoutName()
        {
            // Arrange
            var accountService = new AccountService(ambientDbContextLocator, dbContextScopeFactory);

            // Act // Assert
            await Assert.ThrowsAsync<DbUpdateException>(async () => await accountService.SaveAccount(new Account(new AccountEntity())));
            
        }

        [Fact]
        public async void DeleteAccount_AccountDeleted()
        {
            // Arrange
            var accountService = new AccountService(ambientDbContextLocator, dbContextScopeFactory);

            var testEntry = new AccountEntity
            {
                Name = "Testtext"
            };

            await accountService.SaveAccount(new Account(testEntry));

            // Act
            await accountService.DeleteAccount(new Account(testEntry));

            // Assert
            Assert.Null(await accountService.GetById(testEntry.Id));
        }


        [Fact]
        public async void DeleteAccount_RelatedChargedPaymentsRemoved()
        {
            // Arrange
            var accountService = new AccountService(ambientDbContextLocator, dbContextScopeFactory);
            var paymentService = new PaymentService(ambientDbContextLocator, dbContextScopeFactory);

            var account = new Account(new AccountEntity
            {
                Name = "Testtext"
            });

            await accountService.SaveAccount(account);

            var payment = new Payment(new PaymentEntity
            {
                Note = "Foo",
                ChargedAccount = account.Data
            });

            // Act
            await paymentService.SavePayments(payment);

            Assert.Single(await accountService.GetAllAccounts());
            Assert.Single(await paymentService.GetPaymentsByAccountId(account.Data.Id));
           
            await accountService.DeleteAccount(account);
            
            // Assert
            Assert.False((await accountService.GetAllAccounts()).Any());
            Assert.False((await paymentService.GetPaymentsByAccountId(account.Data.Id)).Any());
        }


        [Fact]
        public async void DeleteAccount_RelatedTargetPaymentSetNull()
        {
            // Arrange
            var accountService = new AccountService(ambientDbContextLocator, dbContextScopeFactory);
            var paymentService = new PaymentService(ambientDbContextLocator, dbContextScopeFactory);

            var chargedAccount = new Account(new AccountEntity
            {
                Name = "Charged"
            });
            var targetAccount = new Account(new AccountEntity
            {
                Name = "Target"
            });

            await accountService.SaveAccount(chargedAccount);
            await accountService.SaveAccount(targetAccount);

            var payment = new Payment(new PaymentEntity
            {
                Note = "Foo",
                ChargedAccount = chargedAccount.Data,
                TargetAccount = targetAccount.Data
            });

            // Act

            await paymentService.SavePayments(payment);
            
            Assert.Equal(2, (await accountService.GetAllAccounts()).Count());
            Assert.Single((await paymentService.GetPaymentsByAccountId(chargedAccount.Data.Id)));

            await accountService.DeleteAccount(targetAccount);
            
            // Assert
            var loadedPayment = await paymentService.GetById(payment.Data.Id);
            Assert.Null(loadedPayment.Data.TargetAccount);
            Assert.NotNull(loadedPayment.Data.ChargedAccount);
        }
    }
}
