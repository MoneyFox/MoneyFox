using FluentAssertions;
using MoneyFox.ConverterLogic;
using MoneyFox.Domain;
using MoneyFox.ViewModels.Payments;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace MoneyFox.Tests.ConverterLogic
{
    [ExcludeFromCodeCoverage]
    public class PaymentAmountConverterLogicTests
    {
        [Theory]
        [InlineData(PaymentType.Income, "+", 2, 2)]
        [InlineData(PaymentType.Expense, "-", 2, 2)]
        [InlineData(PaymentType.Transfer, "-", 2, 2)]
        [InlineData(PaymentType.Transfer, "+", 2, 3)]
        public void GetCorrectSignForExpenseAndIncome(PaymentType type,
            string expectedResult,
            int chargedAccountId,
            int currentAccountId)
        {
            // Arrange
            var payment = new PaymentViewModel
            {
                Type = type, ChargedAccountId = chargedAccountId, CurrentAccountId = currentAccountId
            };

            // Act
            string result = PaymentAmountConverterLogic.GetAmountSign(payment);

            // Assert
            result.Should().StartWith(expectedResult);
        }
    }
}