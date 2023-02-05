namespace MoneyFox.Ui.Tests.Converter;

using System.Globalization;
using FluentAssertions;
using MoneyFox.Domain.Aggregates.AccountAggregate;
using MoneyFox.Ui.Converter;
using MoneyFox.Ui.Views.Payments;
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
        var result = new PaymentAmountConverter().Convert(value: payment, targetType: null!, parameter: null!, culture: CultureInfo.CurrentCulture);

        // Assert
        _ = (result as string).Should().StartWith(expectedResult);
    }
}
