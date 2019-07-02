using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.BusinessDbAccess.PaymentActions;
using MoneyFox.BusinessLogic.PaymentActions;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Foundation;
using Moq;
using Should;
using Xunit;

namespace MoneyFox.BusinessLogic.Tests.PaymentActions
{
    [ExcludeFromCodeCoverage]
    public class ClearPaymentActionTests
    {
        [Fact]
        public async Task ClearPayments_Cleared()
        {
            // Arrange
            var paymentList = new List<Payment>
            {
                new Payment(DateTime.Now.AddDays(1), 100, PaymentType.Expense, new Account("Foo")),
                new Payment(DateTime.Now, 100, PaymentType.Expense, new Account("Foo")),
                new Payment(DateTime.Now.AddDays(-1), 100, PaymentType.Expense, new Account("Foo"))
            };

            var dbAccessMock = new Mock<IClearPaymentDbAccess>();
            dbAccessMock.Setup(x => x.GetUnclearedPayments()).ReturnsAsync(paymentList);

            var clearPaymentAction = new ClearPaymentAction(dbAccessMock.Object);

            // Act
            await clearPaymentAction.ClearPayments();

            // Assert
            paymentList[0].IsCleared.ShouldBeFalse();
            paymentList[1].IsCleared.ShouldBeTrue();
            paymentList[2].IsCleared.ShouldBeTrue();
        }
    }
}
