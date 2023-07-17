namespace MoneyFox.Domain.Tests.Aggregates.RecurringTransactionAggregate;

using Domain.Aggregates.RecurringTransactionAggregate;

internal sealed class RecurringTransactionTests
{
    [Fact]
    public void CreatesAggregate()
    {
        // Arrange

        // Act
        var recurringTransaction = RecurringTransaction.Create(new RecurringTransactionId(10), new DateOnly(2023));
    }
}
