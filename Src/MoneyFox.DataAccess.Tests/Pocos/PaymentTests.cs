using System;
using MoneyFox.DataAccess.Pocos;
using Should;
using Xunit;

namespace MoneyFox.DataAccess.Tests.Pocos
{
    public class PaymentTests
    {
        [Theory]
        [InlineData(-1, true)]
        [InlineData(0, true)]
        [InlineData(1, false)]
        public void ClearPayment_WithoutTimestamps(int days, bool expectedResult)
        {
            // Arrange 
            var payment = new Payment {Data = {Date = DateTime.Now.AddDays(days)}};

            // Act
            payment.ClearPayment();

            // Assert
            Assert.Equal(expectedResult, payment.Data.IsCleared);
        }

        [Theory]
        [InlineData(-120, true)]
        [InlineData(0, true)]
        [InlineData(120, false)]
        public void ClearPayment_WithDifferentTimeStamps(int minutes, bool expectedResult)
        {
            // Arrange 
            var payment = new Payment { Data = { Date = DateTime.Now.AddMinutes(minutes) } };

            // Act
            payment.ClearPayment();

            // Assert
            payment.Data.IsCleared.ShouldEqual(expectedResult);
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
