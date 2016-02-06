using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MvvmCross.Plugins.Sqlite.WindowsUWP;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert;

namespace MoneyManager.Windows.DataAccess.Tests
{
    [TestFixture]
    public class RecurringPaymentDataAccessTests
    {
        private SqliteConnectionCreator connectionCreator;

        [SetUp]
        public void Init()
        {
            connectionCreator = new SqliteConnectionCreator(new WindowsSqliteConnectionFactory());
        }

        [Test]
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

        [Test]
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
    }
}
