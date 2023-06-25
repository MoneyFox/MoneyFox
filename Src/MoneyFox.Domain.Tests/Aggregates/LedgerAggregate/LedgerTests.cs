namespace MoneyFox.Domain.Tests.Aggregates.LedgerAggregate;

using Domain.Aggregates.LedgerAggregate;
using Exceptions;
using FluentAssertions;
using FluentAssertions.Execution;
using Ui.Common.Exceptions;

public class LedgerTests
{
    public class Create
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void RejectCreationIfAccountNameIsInvalid(string name)
        {
            // Act
            var act = () => Ledger.Create(name: name, openingBalance: Money.Zero(Currencies.CHF));

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void CorrectlyCreated()
        {
            // Arrange
            var testLedger = new TestData.SavingsLedger();

            // Act
            var ledger = Ledger.Create(
                name: testLedger.Name,
                openingBalance: testLedger.OpeningBalance,
                note: testLedger.Note,
                isExcluded: testLedger.IsExcludeFromEndOfMonthSummary);

            // Assert
            using (new AssertionScope())
            {
                ledger.Id.Should().Be(new LedgerId());
                ledger.Name.Should().Be(testLedger.Name);
                ledger.OpeningBalance.Should().Be(testLedger.OpeningBalance);
                ledger.CurrentBalance.Should().Be(testLedger.OpeningBalance);
                ledger.Note.Should().Be(testLedger.Note);
                ledger.IsExcludeFromEndOfMonthSummary.Should().Be(testLedger.IsExcludeFromEndOfMonthSummary);
            }
        }
    }

    public class DepositMoney
    {
        [Fact]
        public void IncreaseCurrentBalance()
        {
            // Arrange
            var testLedger = new TestData.SavingsLedger();
            var dbLedger = testLedger.CreateDbLedger();
            var amount = new Money(amount: 100, currency: Currencies.CHF);

            // Act
            dbLedger.Deposit(amount);

            // Assert
            using (new AssertionScope())
            {
                dbLedger.OpeningBalance.Should().Be(testLedger.OpeningBalance);
                dbLedger.CurrentBalance.Should().Be(dbLedger.OpeningBalance + amount);
            }
        }

        [Fact]
        public void ThrowException_IfCurrencyDoesNotMatch()
        {
            // Arrange
            var testLedger = new TestData.SavingsLedger();
            var dbLedger = testLedger.CreateDbLedger();
            var amount = new Money(amount: 100, currency: Currencies.USD);

            // Act
            var act = () => dbLedger.Deposit(amount);

            // Assert
            act.Should().Throw<CurrencyException>();
        }
    }

    public class WithdrawMoney
    {
        [Fact]
        public void ReduceCurrentBalance()
        {
            // Arrange
            var testLedger = new TestData.SavingsLedger();
            var dbLedger = testLedger.CreateDbLedger();
            var amount = new Money(amount: 100, currency: Currencies.CHF);

            // Act
            dbLedger.Withdraw(amount);

            // Assert
            using (new AssertionScope())
            {
                dbLedger.OpeningBalance.Should().Be(testLedger.OpeningBalance);
                dbLedger.CurrentBalance.Should().Be(dbLedger.OpeningBalance - amount);
            }
        }

        [Fact]
        public void ThrowException_IfCurrencyDoesNotMatch()
        {
            // Arrange
            var testLedger = new TestData.SavingsLedger();
            var dbLedger = testLedger.CreateDbLedger();
            var amount = new Money(amount: 100, currency: Currencies.USD);

            // Act
            var act = () => dbLedger.Deposit(amount);

            // Assert
            act.Should().Throw<CurrencyException>();
        }
    }
}
