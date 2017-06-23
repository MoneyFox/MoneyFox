using MoneyFox.DataAccess.Entities;
using MoneyFox.Foundation;
using MoneyFox.Service.Pocos;
using Xunit;

namespace MoneyFox.Service.Tests
{
    public class PaymentAmountHelperTests
    {
        [Theory]
        [InlineData(PaymentType.Expense, 0)]
        [InlineData(PaymentType.Income, 400)]
        public void AddPaymentAmount_AmountAdded(PaymentType type, double expectedResult)
        {
            // Arrange
            var account = new AccountEntity {Name = "Test", CurrentBalance = 200};
            var payment = new Payment
            {
                Data = new PaymentEntity
                {
                    ChargedAccount = account,
                    Amount = 200,
                    Type = type,
                    IsCleared = true
                }
            };

            // Act
           PaymentAmountHelper.AddPaymentAmount(payment);

            // Assert
            Assert.Equal(expectedResult, account.CurrentBalance);
        }

        [Fact]
        public void AddPaymentAmount_AmountTransfer()
        {
            // Arrange
            var chargedAccount = new AccountEntity {Name = "Test", CurrentBalance = 200};
            var targetAccount = new AccountEntity {Name = "Test", CurrentBalance = 0};
            var payment = new Payment
            {
                Data = new PaymentEntity
                {
                    ChargedAccount = chargedAccount,
                    TargetAccount = targetAccount,
                    Amount = 200,
                    Type = PaymentType.Transfer,
                    IsCleared = true
                }
            };

            // Act
           PaymentAmountHelper.AddPaymentAmount(payment);

            // Assert
            Assert.Equal(0, chargedAccount.CurrentBalance);
            Assert.Equal(200, targetAccount.CurrentBalance);
        }

        [Theory]
        [InlineData(PaymentType.Expense, 400)]
        [InlineData(PaymentType.Income, 0)]
        public void RemovePaymentAmount_AmountRemoved(PaymentType type, double expectedResult)
        {
            // Arrange
            var account = new AccountEntity {Name = "Test", CurrentBalance = 200};
            var payment = new Payment
            {
                Data = new PaymentEntity
                {
                    ChargedAccount = account,
                    Amount = 200,
                    Type = type,
                    IsCleared = true
                }
            };

            // Act
           PaymentAmountHelper.RemovePaymentAmount(payment);

            // Assert
            Assert.Equal(expectedResult, account.CurrentBalance);
        }

        [Fact]
        public void RemovePaymentAmount_AmountTransfer()
        {
            // Arrange
            var chargedAccount = new AccountEntity {Name = "Test", CurrentBalance = 0};
            var targetAccount = new AccountEntity {Name = "Test", CurrentBalance = 200};
            var payment = new Payment
            {
                Data = new PaymentEntity
                {
                    ChargedAccount = chargedAccount,
                    TargetAccount = targetAccount,
                    Amount = 200,
                    Type = PaymentType.Transfer,
                    IsCleared = true
                }
            };

            // Act
           PaymentAmountHelper.RemovePaymentAmount(payment);

            // Assert
            Assert.Equal(200, chargedAccount.CurrentBalance);
            Assert.Equal(0, targetAccount.CurrentBalance);
        }
    }
}
