namespace MoneyFox.Tests.Presentation.ConverterLogic
{

    using System.Diagnostics.CodeAnalysis;
    using FluentAssertions;
    using MoneyFox.ConverterLogic;
    using MoneyFox.Core.Aggregates.AccountAggregate;
    using MoneyFox.ViewModels.Payments;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class PaymentAmountConverterLogicTests
    {
        [Theory]
        [InlineData(PaymentType.Income, "+", 2, 2)]
        [InlineData(PaymentType.Expense, "-", 2, 2)]
        [InlineData(PaymentType.Transfer, "-", 2, 2)]
        [InlineData(PaymentType.Transfer, "+", 2, 3)]
        public void GetCorrectSignForExpenseAndIncome(PaymentType type, string expectedResult, int chargedAccountId, int currentAccountId)
        {
            // Arrange
            var payment = new PaymentViewModel { Type = type, ChargedAccountId = chargedAccountId, CurrentAccountId = currentAccountId };

            // Act
            var result = PaymentAmountConverterLogic.GetAmountSign(payment);

            // Assert
            result.Should().StartWith(expectedResult);
        }
    }

}
