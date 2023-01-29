namespace MoneyFox.Domain.Tests.Aggregates.LedgerAggregate;

using Domain.Aggregates.LedgerAggregate;
using FluentAssertions;
using FluentAssertions.Execution;
using TestFramework;

public class TransactionTests
{
    [Fact]
    public void ValidateThatIncomeHasPositiveAmount()
    {
        // Arrange
        var testTransaction = new TestData.SavingsLedger.SalaryTransaction();

        // Act
        var act = () => Transaction.Create(
            TransactionType.Income,
            new Money(-13, Currencies.EUR),
            testTransaction.BookingDate,
            testTransaction.CategoryId,
            testTransaction.Note);

        // Assert
        act.Should().Throw<InvalidTransactionAmountException>();
    }

    [Fact]
    public void ValidateThatExpanseHasNegativeAmount()
    {
        // Arrange
        var testTransaction = new TestData.SavingsLedger.SalaryTransaction();

        // Act
        var act = () => Transaction.Create(
            TransactionType.Expense,
            new Money(13, Currencies.EUR),
            testTransaction.BookingDate,
            testTransaction.CategoryId,
            testTransaction.Note);

        // Assert
        act.Should().Throw<InvalidTransactionAmountException>();
    }

    [Fact]
    public void TransactionCorrectlyCreated()
    {
        // Arrange
        var testTransaction = new TestData.SavingsLedger.SalaryTransaction();

        // Act
        var transaction = Transaction.Create(
            testTransaction.Type,
            testTransaction.Amount,
            testTransaction.BookingDate,
            testTransaction.CategoryId,
            testTransaction.Note);

        // Assert
        using (new AssertionScope())
        {
            transaction.Id.Should().Be(new TransactionId());
            transaction.Type.Should().Be(testTransaction.Type);
            transaction.Amount.Should().Be(testTransaction.Amount);
            transaction.BookingDate.Should().Be(testTransaction.BookingDate);
            transaction.CategoryId.Should().Be(testTransaction.CategoryId);
            transaction.Note.Should().Be(testTransaction.Note);
        }
    }
}
