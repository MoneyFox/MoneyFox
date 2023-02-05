namespace MoneyFox.Domain.Tests.Aggregates.LedgerAggregate;

using System.Collections.Immutable;
using Domain.Aggregates.LedgerAggregate;
using FluentAssertions;
using FluentAssertions.Execution;
using TestFramework;

public class LedgerTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void RejectCreationIfAccountNameIsInvalid(string name)
    {
        // Act
        var act = () => Ledger.Create(name, Money.Zero(Currencies.CHF));

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void CorrectlyCreated()
    {
        // Arrange
        var testLedger = new TestData.SavingsLedger();

        // Act
        var ledger = Ledger.Create(testLedger.Name, testLedger.CurrentBalance, testLedger.Note, testLedger.IsExcluded);

        // Assert
        using (new AssertionScope())
        {
            ledger.Id.Should().Be(new LedgerId());
            ledger.Name.Should().Be(testLedger.Name);
            ledger.CurrentBalance.Should().Be(testLedger.CurrentBalance);
            ledger.Note.Should().Be(testLedger.Note);
            ledger.ExcludeFromEndOfMonthSummary.Should().Be(testLedger.IsExcluded);
        }
    }

    [Fact]
    public void ExpenseCorrectlyAddedAndLedgerBalanceAdjusted()
    {
        // Arrange
        var testLedger = new TestData.SavingsLedger { CurrentBalance = new(1000, Currencies.CHF), Transactions = ImmutableList<TestData.ILedger.ITransaction>.Empty };
        var ledgerAggregate = testLedger.CreateDbLedger();
        var expense = new TestData.SavingsLedger.BeverageTransaction { Amount = new Money(-200, Currencies.CHF) };

        // Act
        ledgerAggregate.AddTransaction(
            expense.Reference,
            expense.Type,
            expense.Amount,
            expense.BookingDate,
            expense.CategoryId,
            expense.Note);

        ledgerAggregate.Transactions.Should().ContainSingle();
        ledgerAggregate.CurrentBalance.Amount.Should().Be(testLedger.CurrentBalance + expense.Amount);
        ledgerAggregate.CurrentBalance.Currency.Should().Be(testLedger.CurrentBalance.Currency);
    }

    [Fact]
    public void IncomeCorrectlyAddedAndLedgerBalanceAdjusted()
    {
        // Arrange
        var testLedger = new TestData.SavingsLedger { CurrentBalance = new Money(1000, Currencies.CHF), Transactions = ImmutableList<TestData.ILedger.ITransaction>.Empty };
        var ledgerAggregate = testLedger.CreateDbLedger();
        var income = new TestData.SavingsLedger.SalaryTransaction { Amount = new Money(300, Currencies.CHF) };

        // Act
        ledgerAggregate.AddTransaction(
            income.Reference,
            income.Type,
            income.Amount,
            income.BookingDate,
            income.CategoryId,
            income.Note);

        ledgerAggregate.Transactions.Should().ContainSingle();
        ledgerAggregate.CurrentBalance.Amount.Should().Be(testLedger.CurrentBalance + income.Amount);
        ledgerAggregate.CurrentBalance.Currency.Should().Be(testLedger.CurrentBalance.Currency);
    }
}
