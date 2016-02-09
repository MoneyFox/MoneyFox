using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MvvmCross.Plugins.Sqlite.WindowsUWP;

namespace MoneyManager.Windows.DataAccess.Tests
{
    [TestClass]
    public class PaymentDataAccessTests
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
            var dataAccess = new PaymentDataAccess(connectionCreator);
            var list = dataAccess.LoadList();

            foreach (var payment in list)
            {
                dataAccess.DeleteItem(payment);
            }
        }

        [TestMethod]
        public void SaveToDatabase_NewPayment_CorrectId()
        {
            var amount = 789;

            var payment = new Payment
            {
                Amount = amount
            };

            new PaymentDataAccess(connectionCreator).SaveItem(payment);

            Assert.AreEqual(1, payment.Id);
            Assert.AreEqual(amount, payment.Amount);
        }

        [TestMethod]
        public void SaveToDatabase_ExistingPayment_CorrectId()
        {
            var payment = new Payment();

            var dataAccess = new PaymentDataAccess(connectionCreator);
            dataAccess.SaveItem(payment);

            Assert.AreEqual(0, payment.Amount);

            var id = payment.Id;

            var amount = 789;
            payment.Amount = amount;

            Assert.AreEqual(1, payment.Id);
            Assert.AreEqual(amount, payment.Amount);
        }

        [TestMethod]
        public void SaveToDatabase_MultiplePayment_AllSaved()
        {
            var payment1 = new Payment
            {
               Amount = 123,
            };

            var payment2 = new Payment
            {
                Amount = 789,
            };

            var dataAccess = new PaymentDataAccess(connectionCreator);
            dataAccess.SaveItem(payment1);
            dataAccess.SaveItem(payment2);

            var resultList = dataAccess.LoadList();

            Assert.AreEqual(2, resultList.Count);
        }
    }
}
