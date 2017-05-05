
using MoneyFox.Business.ViewModels;
using Xunit;

namespace MoneyFox.Business.Tests.ViewModels
{
    public class RecurringPaymentViewModelTests
    {
        [Fact]
        public void ChargedAccount_Default_Null()
        {
            Assert.Null(new RecurringPaymentViewModel().ChargedAccount);
        }

        [Fact]
        public void TargetAccount_Default_Null()
        {
            Assert.Null(new RecurringPaymentViewModel().TargetAccount);
        }

        [Fact]
        public void Category_Default_Null()
        {
            Assert.Null(new RecurringPaymentViewModel().Category);
        }
    }
}
