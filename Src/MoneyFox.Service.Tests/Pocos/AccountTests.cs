using MoneyFox.DataAccess.Pocos;
using Xunit;

namespace MoneyFox.Service.Tests.Pocos
{
    public class AccountTests
    {
        [Fact]
        public void Ctor_ListsNotNull()
        {
            // Act
            var account = new Account();

            // Assert
            Assert.NotNull(account.Data.ChargedPayments);
            Assert.NotNull(account.Data.TargetedPayments);
            Assert.NotNull(account.Data.ChargedRecurringPayments);
            Assert.NotNull(account.Data.TargetedRecurringPayments);
        }
    }
}
