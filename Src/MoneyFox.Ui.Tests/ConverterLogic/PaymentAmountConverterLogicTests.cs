namespace MoneyFox.Ui.Tests.ConverterLogic;

using Common.ConverterLogic;
using Domain.Aggregates.AccountAggregate;
using FluentAssertions;
using Ui.Views.Payments;
using Views.Payments;
using Xunit;

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
        _ = result.Should().StartWith(expectedResult);
    }
}
