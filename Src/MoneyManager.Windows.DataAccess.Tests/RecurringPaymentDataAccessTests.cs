using MoneyManager.DataAccess;
using MoneyManager.Foundation.Model;
using Xunit;
using XunitShouldExtension;

namespace MoneyManager.Windows.DataAccess.Tests
{
    public class RecurringPaymentDataAccessTests
    {
        [Fact]
        public void SaveToDatabase_NewRecurringPayment_CorrectId()
        {
            var amount = 789;

            var payment = new RecurringPayment
            {
                Amount = amount
            };

            new RecurringPaymentDataAccess(new WindowsSqliteConnectionFactory()).SaveItem(payment);

            payment.Id.ShouldBeGreaterThanOrEqualTo(1);
            payment.Amount.ShouldBe(amount);
        }

        [Fact]
        public void SaveToDatabase_ExistingRecurringPayment_CorrectId()
        {
            var payment = new RecurringPayment();

            var dataAccess = new RecurringPaymentDataAccess(new WindowsSqliteConnectionFactory());
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
