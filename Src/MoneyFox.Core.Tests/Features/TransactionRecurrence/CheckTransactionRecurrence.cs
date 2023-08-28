namespace MoneyFox.Core.Tests.Features.TransactionRecurrence;

using Core.Common;
using Core.Features.RecurringTransactionCreation;
using Core.Features.TransactionRecurrence;
using Domain.Aggregates.RecurringTransactionAggregate;
using Domain.Tests.TestFramework;
using MediatR;

public class CheckTransactionRecurrenceHandlerTest : InMemoryTestBase
{
    private readonly CheckTransactionRecurrence.Handler handler;
    private readonly ISender sender;
    private readonly ISystemDateHelper systemDateHelper;

    public CheckTransactionRecurrenceHandlerTest()
    {
        sender = Substitute.For<ISender>();
        systemDateHelper = Substitute.For<ISystemDateHelper>();
        systemDateHelper.TodayDateOnly.Returns(DateOnly.FromDateTime(DateTime.Today));
        handler = new(dbContext: Context, sender: sender, systemDateHelper: systemDateHelper);
    }

    [Fact]
    public async Task CreateAllMissedRecurrences()
    {
        // Arrange
        systemDateHelper.TodayDateOnly.Returns(DateOnly.FromDateTime(DateTime.Today.AddDays(2)));
        var recurringTransaction = new TestData.RecurringExpense { Recurrence = Recurrence.Daily };
        Context.RegisterRecurringTransaction(recurringTransaction);

        // Act
        await handler.Handle(command: new(), cancellationToken: default);

        // Assert
        await sender.Received(2).Send(Arg.Any<CreateRecurringTransaction.Command>());
    }

    [Theory]
    [InlineData(Recurrence.Daily, 1)]
    [InlineData(Recurrence.Weekly, 7)]
    [InlineData(Recurrence.Biweekly, 14)]
    [InlineData(Recurrence.Monthly, 31)]
    [InlineData(Recurrence.Bimonthly, 62)]
    [InlineData(Recurrence.Quarterly, 93)]
    [InlineData(Recurrence.Biannually, 190)]
    [InlineData(Recurrence.Yearly, 370)]
    public async Task CreatePaymentsForDifferentRecurrences(Recurrence recurrence, int days)
    {
        // Arrange
        systemDateHelper.TodayDateOnly.Returns(DateOnly.FromDateTime(DateTime.Today.AddDays(days)));
        var recurringTransaction = new TestData.RecurringExpense { Recurrence = recurrence };
        Context.RegisterRecurringTransaction(recurringTransaction);

        // Act
        await handler.Handle(command: new(), cancellationToken: default);

        // Assert
        await sender.Received(1).Send(Arg.Any<CreateRecurringTransaction.Command>());
    }

    [Fact]
    public async Task SkipRecurringTransactionsWithEndDateInPast()
    {
        // Arrange
        systemDateHelper.TodayDateOnly.Returns(DateOnly.FromDateTime(DateTime.Today.AddDays(1)));
        var recurringTransaction1 = new TestData.RecurringExpense { Recurrence = Recurrence.Daily };
        var recurringTransaction2 = new TestData.RecurringExpense { Recurrence = Recurrence.Daily };
        var recurringTransaction3 = new TestData.RecurringExpense { Recurrence = Recurrence.Daily, EndDate = DateOnly.FromDateTime(DateTime.Today) };
        Context.RegisterRecurringTransactions(recurringTransaction1, recurringTransaction2, recurringTransaction3);

        // Act
        await handler.Handle(command: new(), cancellationToken: default);

        // Assert
        await sender.Received(2).Send(Arg.Any<CreateRecurringTransaction.Command>());
    }
}
