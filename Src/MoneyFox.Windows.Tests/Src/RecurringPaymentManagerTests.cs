using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyFox.Shared;
using MoneyFox.Shared.DataAccess;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Manager;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Repositories;
using MvvmCross.Plugins.File.WindowsCommon;
using MvvmCross.Plugins.Sqlite.WindowsUWP;
using System;

namespace MoneyFox.Windows.Tests.Src
{
    [TestClass]
    public class RecurringPaymentManagerTests
    {
        [TestMethod]        
        public void CheckRecurringPayments_Daily_CorrectPaymentsCreated()
        {
            //Setup
            var sqliteConnector = new SqliteConnectionCreator(new WindowsSqliteConnectionFactory(), 
                new MvxWindowsCommonFileStore());

            var accountRepo = new AccountRepository(new AccountDataAccess(sqliteConnector));

            var paymentRepo = new PaymentRepository(new PaymentDataAccess(sqliteConnector),
                new RecurringPaymentDataAccess(sqliteConnector),
                accountRepo,
                new CategoryRepository(new CategoryDataAccess(sqliteConnector)));

            var account = new Account
            {
                Name = "test1",
                CurrentBalance = 100
            };

            accountRepo.Save(account);

            var payment = new Payment
            {
                ChargedAccount = account,
                Amount = 10,
                Date = DateTime.Today.AddDays(-1),
                IsRecurring = true
            };

            payment.RecurringPayment =
                RecurringPaymentHelper.
                    GetRecurringFromPayment(payment,
                        true,
                        (int) PaymentRecurrence.Daily,
                        DateTime.Now.AddYears(1));

            paymentRepo.Save(payment);

            //Execution
            new RecurringPaymentManager(paymentRepo, accountRepo).CheckRecurringPayments();

            //Assert
            Assert.AreEqual(90, account.CurrentBalance);
            Assert.AreEqual(90, accountRepo.Data[0].CurrentBalance);

            Assert.AreEqual(2, paymentRepo.Data.Count);

            Assert.IsTrue(paymentRepo.Data[0].IsCleared);
            Assert.IsTrue(paymentRepo.Data[1].IsCleared);

            Assert.IsTrue(paymentRepo.Data[0].IsRecurring);
            Assert.IsTrue(paymentRepo.Data[1].IsRecurring);

            Assert.AreNotEqual(0, paymentRepo.Data[0].RecurringPaymentId);
            Assert.AreNotEqual(0, paymentRepo.Data[1].RecurringPaymentId);

            Assert.IsNotNull(paymentRepo.Data[0].RecurringPayment);
            Assert.IsNotNull(paymentRepo.Data[1].RecurringPayment);
        }
    }
}
