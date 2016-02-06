using MoneyManager.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MvvmCross.Plugins.Sqlite.WindowsUWP;
using NUnit.Framework;

namespace MoneyManager.Windows.DataAccess.Tests
{
    [TestFixture()]
    public class PaymentDataAccessTests
    {
        private SqliteConnectionCreator connectionCreator;

        [SetUp]
        public void Init()
        {
            connectionCreator = new SqliteConnectionCreator(new WindowsSqliteConnectionFactory());
        }

        [Test]
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

        [Test]
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
