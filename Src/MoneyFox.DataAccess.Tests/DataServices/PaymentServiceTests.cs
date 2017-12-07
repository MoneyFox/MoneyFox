using System;
using System.IO;
using EntityFramework.DbContextScope;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Business.ViewModels;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Constants;
using Xunit;

namespace MoneyFox.DataAccess.Tests.DataServices
{
    /// <summary>
    ///     This test is in the DataAccess project, because it tests the direct access to the database.
    /// </summary>
    public class PaymentServiceTests : IDisposable
    {
        /// <summary>
        ///     Setup Logic who is executed before every test
        /// </summary>
        public PaymentServiceTests()
        {
            ApplicationContext.DbPath = Path.Combine(AppContext.BaseDirectory, DatabaseConstants.DB_NAME);
            Dispose();

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

        private readonly DbContextScopeFactory dbContextScopeFactory;
        private readonly AmbientDbContextLocator ambientDbContextLocator;

        [Fact]
        public async void SaveAccount_AccountSavedAndLoaded()
        {
            // Arrange
            AccountEntity testAccount;

            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    testAccount = new AccountEntity { Name = "testAccount" };
                    dbContext.Accounts.Add(testAccount);
                    await dbContextScope.SaveChangesAsync();
                }
            }

            var paymentService = new PaymentService(ambientDbContextLocator, dbContextScopeFactory);

            var testEntry = new Payment(new PaymentEntity
            {
                ChargedAccount = testAccount
            });

            // Act
            await paymentService.SavePayments(testEntry);

            // Assert
            Assert.Equal(testEntry.Data.Amount, (await paymentService.GetById(testEntry.Data.Id)).Data.Amount);
        }

        [Fact]
        public async void SaveAccount_AccountSavedIdSet()
        {
            // Arrange
            AccountEntity testAccount;

            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    testAccount = new AccountEntity { Name = "testAccount" };
                    dbContext.Accounts.Add(testAccount);
                    await dbContextScope.SaveChangesAsync();
                }
            }

            var paymentService = new PaymentService(ambientDbContextLocator, dbContextScopeFactory);

            var testEntry = new Payment(new PaymentEntity
            {
                ChargedAccount = testAccount
            });

            // Act
            await paymentService.SavePayments(testEntry);
            
            // Assert
            Assert.NotEqual(0, testEntry.Data.Id);
        }

        [Fact]
        public async void SavePayment_MultiplePaymentSavedAndLoaded()
        {
            // Arrange
            AccountEntity testAccount;

            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    testAccount = new AccountEntity { Name = "testAccount" };
                    dbContext.Accounts.Add(testAccount);
                    await dbContextScope.SaveChangesAsync();
                }
            }

            var paymentService = new PaymentService(ambientDbContextLocator, dbContextScopeFactory);

            var testEntry1 = new Payment(new PaymentEntity
            {
                ChargedAccount = testAccount,
                Amount = 312
            });
            var testEntry2 = new Payment(new PaymentEntity
            {
                ChargedAccount = testAccount,
                Amount = 531
            });

            // Act
            await paymentService.SavePayments(testEntry1);
            await paymentService.SavePayments(testEntry2);

            // Assert
            Assert.Equal(testEntry1.Data.Amount, (await paymentService.GetById(testEntry1.Data.Id)).Data.Amount);
            Assert.Equal(testEntry2.Data.Amount, (await paymentService.GetById(testEntry2.Data.Id)).Data.Amount);
        }

        [Fact]
        public async void SavePayment_PaymentUpdatedCorrectly()
        {
            // Arrange
            const int updatedAmount = 7845;
            AccountEntity testAccount;

            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    testAccount = new AccountEntity { Name = "testAccount" };
                    dbContext.Accounts.Add(testAccount);
                    await dbContextScope.SaveChangesAsync();
                }
            }

            var paymentService = new PaymentService(ambientDbContextLocator, dbContextScopeFactory);

            var testEntry = new Payment(new PaymentEntity
            {
                ChargedAccount = testAccount,
                Amount = 123
            });

            await paymentService.SavePayments(testEntry);
            testEntry.Data.Amount = updatedAmount;

            // Act
            await paymentService.SavePayments(testEntry);

            // Assert
            var loadedEntry = await paymentService.GetById(testEntry.Data.Id);
            Assert.Equal(updatedAmount, (await paymentService.GetById(testEntry.Data.Id)).Data.Amount);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async void SavePayments_WithRecurringPayment_GetRecurringPaymentFromHelper()
        {
            // Arrange
            AccountEntity testAccount;

            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    testAccount = new AccountEntity {Name = "testAccount"};
                    dbContext.Accounts.Add(testAccount);
                    await dbContextScope.SaveChangesAsync();
                }
            }

            var testEntry = new PaymentViewModel(new Payment
                {
                    Data =
                    {
                        ChargedAccount = testAccount,
                        Date = DateTime.Now,
                        IsRecurring = true,
                        Note = "Testtext"
                    }
                });
                testEntry.RecurringPayment = new RecurringPaymentViewModel(
                    RecurringPaymentHelper.GetRecurringFromPayment(testEntry.Payment,
                                                                   true,
                                                                   PaymentRecurrence.Bimonthly,
                                                                   DateTime.Now));

                var paymentService = new PaymentService(ambientDbContextLocator, dbContextScopeFactory);

                // Act
                await paymentService.SavePayments(testEntry.Payment);
                var payment = await paymentService.GetById(testEntry.Payment.Data.Id);

                // Assert
                Assert.NotNull(payment);
            }
        }
    }