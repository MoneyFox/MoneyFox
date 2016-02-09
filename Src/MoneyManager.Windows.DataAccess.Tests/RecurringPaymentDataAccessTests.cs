using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MvvmCross.Plugins.Sqlite.WindowsUWP;

namespace MoneyManager.Windows.DataAccess.Tests
{
    [TestClass]
    public class RecurringPaymentDataAccessTests
    {
        private SqliteConnectionCreator connectionCreator;

        [TestInitialize]
        public void Init()
        {
            connectionCreator = new SqliteConnectionCreator(new WindowsSqliteConnectionFactory());
        }

        [TestCleanup]
        public void Cleanup()
        {
            var dataAccess = new RecurringPaymentDataAccess(connectionCreator);
            var list = dataAccess.LoadList();

            foreach (var payment in list)
            {
                dataAccess.DeleteItem(payment);
            }
        }

        [TestMethod]
        public void SaveToDatabase_NewRecurringPayment_CorrectId()
        {
            var amount = 789;

            var payment = new RecurringPayment
            {
                Amount = amount
            };

            new RecurringPaymentDataAccess(connectionCreator).SaveItem(payment);

            Assert.AreEqual(1, payment.Id);
            Assert.AreEqual(amount, payment.Amount);
        }

        [TestMethod]
        public void SaveToDatabase_ExistingRecurringPayment_CorrectId()
        {
            var payment = new RecurringPayment();

            var dataAccess = new RecurringPaymentDataAccess(connectionCreator);
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
                Amount = 123,
            };

            var payment2 = new RecurringPayment
            {
                Amount = 789,
            };

            var dataAccess = new RecurringPaymentDataAccess(connectionCreator);
            dataAccess.SaveItem(payment1);
            dataAccess.SaveItem(payment2);

            var resultList = dataAccess.LoadList();

            Assert.AreEqual(2, resultList.Count);
        }
    }
}
