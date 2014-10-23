using System;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;

namespace MoneyManager.DataAccess.WindowsPhone.Test
{
    [TestClass]
    public class RecurringTransactionDataAccessTest
    {
        [TestInitialize]
        public void TestInit()
        {
            using (var db = SqlConnectionFactory.GetSqlConnection())
            {
                db.CreateTable<RecurringTransaction>();
            }
        }

        [TestMethod]
        public void CrudRecurringTransactionTest()
        {
            var recurringTransactionDataAccess = new RecurringTransactionDataAccess();

            const double firstAmount = 100.70;
            const double secondAmount = 80.45;

            var transaction = new RecurringTransaction
            {
                ChargedAccountId = 7,
                Amount = firstAmount,
                StartDate = DateTime.Today,
                EndDate = DateTime.Now.AddDays(7),
                Note = "this is a note!!!",
            };

            recurringTransactionDataAccess.Save(transaction);

            var list = recurringTransactionDataAccess.LoadList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(firstAmount, list.First().Amount);

            transaction.Amount = secondAmount;

            recurringTransactionDataAccess.Update(transaction);

            list = recurringTransactionDataAccess.LoadList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(secondAmount, list.First().Amount);

            recurringTransactionDataAccess.Delete(transaction);

            list = recurringTransactionDataAccess.LoadList();
            Assert.IsFalse(list.Any());

        }

    }
}
