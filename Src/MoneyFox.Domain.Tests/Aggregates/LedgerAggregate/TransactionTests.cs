﻿namespace MoneyFox.Domain.Tests.Aggregates.LedgerAggregate;

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
            reference: testTransaction.Reference,
            type: TransactionType.Income,
            amount: new(amount: -13, currency: Currencies.EUR),
            ledgerBalance: new (50, Currencies.EUR),
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
        var testTransaction = new TestData.SavingsLedger.SalaryTransaction();

        // Act
        var act = () => Transaction.Create(
            reference: testTransaction.Reference,
            type: TransactionType.Expense,
            amount: new(amount: 13, currency: Currencies.EUR),
            ledgerBalance: new (50, Currencies.EUR),
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
        var testTransaction = new TestData.SavingsLedger.SalaryTransaction();

        // Act
        var transaction = Transaction.Create(
            reference: testTransaction.Reference,
            type: testTransaction.Type,
            amount: testTransaction.Amount,
            ledgerBalance: testTransaction.LedgerBalance,
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
            transaction.LedgerBalance.Should().Be(testTransaction.LedgerBalance);
            transaction.LedgerBalance.Currency.Should().Be(testTransaction.Amount.Currency);
            transaction.BookingDate.Should().Be(testTransaction.BookingDate);
            transaction.CategoryId.Should().Be(testTransaction.CategoryId);
            transaction.Note.Should().Be(testTransaction.Note);
        }
    }
}