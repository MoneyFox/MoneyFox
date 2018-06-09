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

        /// <summary>
        ///     This test might fail around midnight.
        /// </summary>
        [Theory]
        [InlineData(-120)]
        [InlineData(0)]
        [InlineData(120)]
        public void ClearPayment_WithDifferentTimeStamps(int minutes)
        {
            // Arrange 
            var payment = new Payment { Data = { Date = DateTime.Now.AddMinutes(minutes) } };

            // Act
            payment.ClearPayment();

            // Assert
            payment.Data.IsCleared.ShouldBeTrue();
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
