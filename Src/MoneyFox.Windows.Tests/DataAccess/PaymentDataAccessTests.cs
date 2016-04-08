using System;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyFox.Shared;
using MoneyFox.Shared.DataAccess;
using MoneyFox.Shared.Model;
using MvvmCross.Plugins.Sqlite.WindowsUWP;

namespace MoneyFox.Windows.Tests.DataAccess
{
    [TestClass]
    public class PaymentDataAccessTests
    {
        private SqliteConnectionCreator _connectionCreator;

        [TestInitialize]
        public void Init()
        {
            _connectionCreator = new SqliteConnectionCreator(new WindowsSqliteConnectionFactory());
        }

        [TestMethod]
        public void SaveToDatabase_NewPayment_CorrectId()
        {
            var amount = 789;

            var payment = new Payment
            {
                Amount = amount
            };

            new PaymentDataAccess(_connectionCreator).SaveItem(payment);

            Assert.IsTrue(payment.Id >= 1);
            Assert.AreEqual(amount, payment.Amount);
        }

        [TestMethod]
        public void SaveToDatabase_ExistingPayment_CorrectId()
        {
            var payment = new Payment();

            var dataAccess = new PaymentDataAccess(_connectionCreator);
            dataAccess.SaveItem(payment);

            Assert.AreEqual(0, payment.Amount);

            var id = payment.Id;

            var amount = 789;
            payment.Amount = amount;

            Assert.AreEqual(id, payment.Id);
            Assert.AreEqual(amount, payment.Amount);
        }
        [TestMethod]
        public void SaveToDatabase_MultipleRecurringPayment_AllSaved()
        {
            var payment1 = new RecurringPayment
            {
                Note = "MultiPayment1",
            };

            var payment2 = new RecurringPayment
            {
                Note = "MultiPayment2",
            };

            var dataAccess = new RecurringPaymentDataAccess(_connectionCreator);
            dataAccess.SaveItem(payment1);
            dataAccess.SaveItem(payment2);

            var resultList = dataAccess.LoadList();

            Assert.IsTrue(resultList.Any(x => x.Id == payment1.Id && x.Note == payment1.Note));
            Assert.IsTrue(resultList.Any(x => x.Id == payment2.Id && x.Note == payment2.Note));
        }

        [TestMethod]
        public void SaveToDatabase_CreateAndUpdatePayment_CorrectlyUpdated()
        {
            var firstAmount = 5555555;
            var secondAmount = 222222222;

            var payment = new Payment
            {
                Amount = firstAmount
            };

            var dataAccess = new PaymentDataAccess(_connectionCreator);
            dataAccess.SaveItem(payment);

            Assert.AreEqual(firstAmount, dataAccess.LoadList().FirstOrDefault(x => x.Id == payment.Id).Amount);

            payment.Amount = secondAmount;
            dataAccess.SaveItem(payment);

            var categories = dataAccess.LoadList();
            Assert.IsFalse(categories.Any(x => Math.Abs(x.Amount - firstAmount) < 0.1));
            Assert.AreEqual(secondAmount, categories.First(x => x.Id == payment.Id).Amount);
        }

        [TestMethod]
        public void DeleteFromDatabase_PaymentToDelete_CorrectlyDelete()
        {
            var payment = new Payment
            {
                Note = "paymentToDelete",
            };

            var dataAccess = new PaymentDataAccess(_connectionCreator);
            dataAccess.SaveItem(payment);

            Assert.IsTrue(dataAccess.LoadList(x => x.Id == payment.Id).Any());

            dataAccess.DeleteItem(payment);
            Assert.IsFalse(dataAccess.LoadList(x => x.Id == payment.Id).Any());
        }
    }
}
