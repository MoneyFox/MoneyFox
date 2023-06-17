namespace MoneyFox.Domain.Tests.Aggregates.TransactionAggregate;

using FluentAssertions;
using FluentAssertions.Execution;
using MoneyFox.Domain.Aggregates.TransactionAggregate;
using MoneyFox.Domain.Tests.TestFramework;

public class TransactionTests
{
    [Fact]
    public void ValidateThatIncomeHasPositiveAmount()
    {
        // Arrange
        var testTransaction = new TestData.SalaryTransaction();

        // Act
        var act = () => Transaction.Create(
            reference: testTransaction.Reference,
            testTransaction.LedgerId,
            type: TransactionType.Income,
            amount: new(amount: -13, currency: Currencies.EUR),
            bookingDate: testTransaction.BookingDate,
            categoryId: testTransaction.CategoryId,
            note: testTransaction.Note);

        // Assert
        act.Should().Throw<InvalidTransactionAmountException>();
    }

    [Fact]
    public void ValidateThatExpanseHasNegativeAmount()
    {
        // Arrange
        var testTransaction = new TestData.SalaryTransaction();

        // Act
        var act = () => Transaction.Create(
            reference: testTransaction.Reference,
            testTransaction.LedgerId,
            type: TransactionType.Expense,
            amount: new(amount: 13, currency: Currencies.EUR),
            bookingDate: testTransaction.BookingDate,
            categoryId: testTransaction.CategoryId,
            note: testTransaction.Note);

        // Assert
        act.Should().Throw<InvalidTransactionAmountException>();
    }

    [Fact]
    public void TransactionCorrectlyCreated()
    {
        // Arrange
        var testTransaction = new TestData.SalaryTransaction();

        // Act
        var transaction = Transaction.Create(
            reference: testTransaction.Reference,
            testTransaction.LedgerId,
            type: testTransaction.Type,
            amount: testTransaction.Amount,
            bookingDate: testTransaction.BookingDate,
            categoryId: testTransaction.CategoryId,
            note: testTransaction.Note);

        // Assert
        using (new AssertionScope())
        {
            transaction.Id.Should().Be(new TransactionId());
            transaction.Reference.Should().Be(testTransaction.Reference);
            transaction.Type.Should().Be(testTransaction.Type);
            transaction.Amount.Should().Be(testTransaction.Amount);
            transaction.BookingDate.Should().Be(testTransaction.BookingDate);
            transaction.CategoryId.Should().Be(testTransaction.CategoryId);
            transaction.Note.Should().Be(testTransaction.Note);
        }
    }
}
