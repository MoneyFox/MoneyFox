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

        [TestMethod]
        public void SaveToDatabase_NewPayment_CorrectId()
        {
            var amount = 789;

            var payment = new Payment()
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
    }
}
