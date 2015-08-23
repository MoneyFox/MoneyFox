using System;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Core;
using MoneyManager.Core.DataAccess;
using MoneyManager.Foundation.Model;
using MoneyManager.Windows.Core.Tests.Helper;
using SQLite.Net.Platform.WinRT;

namespace MoneyManager.Windows.Core.Tests.DataAccess
{
    [TestClass]
    public class RecurringTransactionDataAccessTest
    {
        [TestMethod]
        [TestCategory("Integration")]
        public void RecurringTransactionDataAccess_CrudRecurringTransaction()
        {
            var recurringTransactionDataAccess =
                new RecurringTransactionDataAccess(new DbHelper(new SQLitePlatformWinRT(), new TestDatabasePath()));

            const double firstAmount = 100.70;
            const double secondAmount = 80.45;

            var transaction = new RecurringTransaction
            {
                ChargedAccountId = 7,
                Amount = firstAmount,
                StartDate = DateTime.Today,
                EndDate = DateTime.Now.AddDays(7),
                Note = "this is a note!!!"
            };

            recurringTransactionDataAccess.Save(transaction);


            var list = recurringTransactionDataAccess.LoadList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(firstAmount, list.First().Amount);

            transaction.Amount = secondAmount;

            recurringTransactionDataAccess.Save(transaction);

            recurringTransactionDataAccess.LoadList();
            list = recurringTransactionDataAccess.LoadList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(secondAmount, list.First().Amount);

            recurringTransactionDataAccess.Delete(transaction);

            recurringTransactionDataAccess.LoadList();
            list = recurringTransactionDataAccess.LoadList();
            Assert.IsFalse(list.Any());
        }
    }
}