using MoneyManager.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MvvmCross.Plugins.Sqlite.WindowsUWP;
using Xunit;
using XunitShouldExtension;

namespace MoneyManager.Windows.DataAccess.Tests
{
    public class PaymentDataAccessTests
    {
        [Fact]
        public void SaveToDatabase_NewPayment_CorrectId()
        {
            var amount = 789;

            var payment = new Payment()
            {
                Amount = amount
            };

            new PaymentDataAccess(new SqliteConnectionCreator(new WindowsSqliteConnectionFactory())).SaveItem(payment);

            payment.Id.ShouldBeGreaterThanOrEqualTo(1);
            payment.Amount.ShouldBe(amount);
        }

        [Fact]
        public void SaveToDatabase_ExistingPayment_CorrectId()
        {
            var payment = new Payment();

            var dataAccess = new PaymentDataAccess(new SqliteConnectionCreator(new WindowsSqliteConnectionFactory()));
            dataAccess.SaveItem(payment);

            payment.Amount.ShouldBe(0);
            var id = payment.Id;

            var amount = 789;
            payment.Amount = amount;

            payment.Id.ShouldBe(id);
            payment.Amount.ShouldBe(amount);
        }
    }
}
