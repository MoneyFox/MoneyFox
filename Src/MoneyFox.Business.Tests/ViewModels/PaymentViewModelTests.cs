using System.Diagnostics.CodeAnalysis;
using MoneyFox.Business.ViewModels;
using Xunit;

namespace MoneyFox.Business.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    [Collection("MvxIocCollection")]
    public class PaymentViewModelTests
    {
        [Fact]
        public void ChargedAccount_Default_Null()
        {
            Assert.Null(new PaymentViewModel().ChargedAccount);
        }

        [Fact]
        public void TargetAccount_Default_Null()
        {
            Assert.Null(new PaymentViewModel().TargetAccount);
        }

        [Fact]
        public void RecurringPayment_Default_Null()
        {
            Assert.Null(new PaymentViewModel().RecurringPayment);
        }

        [Fact]
        public void Category_Default_Null()
        {
            Assert.Null(new PaymentViewModel().Category);
        }
    }
}
