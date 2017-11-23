using System;
using MoneyFox.DataAccess.Pocos;
using Xunit;

namespace MoneyFox.Service.Tests.Pocos
{
    public class PaymentTests
    {
        [Theory]
        [InlineData(-1, true)]
        [InlineData(0, true)]
        [InlineData(1, false)]
        public void ClearPayment(int days, bool expectedResult)
        {
            // Arrange 
            var payment = new Payment {Data = {Date = DateTime.Now.AddDays(days)}};

            // Act
            payment.ClearPayment();

            // Assert
            Assert.Equal(expectedResult, payment.Data.IsCleared);
        }

        [Fact]
        public void ClearPayment_AlreadyCleared()
        {
            // Arrange 
            var payment = new Payment {Data = {Date = DateTime.Now.AddDays(3), IsCleared = true}};

            // Act
            payment.ClearPayment();

            // Assert
            Assert.True(payment.Data.IsCleared);
        }
    }
}
