using System;
using MoneyFox.DataLayer.Entities;
using MoneyFox.Foundation;
using Xunit;

namespace MoneyFox.DataLayer.Tests.Entities
{
    public class PaymentTests
    {
        [Fact]
        public void Ctor_ChargedAccountNull_ArgumentNullException()
        {
            // Arrange

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => new Payment(DateTime.Now, 123, PaymentType.Expense, "note", null));
        }
    }
}
