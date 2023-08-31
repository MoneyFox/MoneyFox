namespace MoneyFox.Core.Tests.Features.TransactionRecurrence;

using Core.Common;
using Core.Common.Extensions;
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
        systemDateHelper.TodayDateOnly.Returns(DateTime.Today.AddDays(days).ToDateOnly());
        var recurringTransaction = new TestData.RecurringExpense { Recurrence = recurrence };
        Context.RegisterRecurringTransaction(recurringTransaction);

        // Act
        await handler.Handle(command: new(), cancellationToken: default);

        // Assert
        await sender.Received().Send(Arg.Any<CreateRecurringTransaction.Command>());
    }

    [Fact]
    public async Task SkipRecurringTransactionsWithEndDateInPast()
    {
        // Arrange
        systemDateHelper.TodayDateOnly.Returns(DateOnly.FromDateTime(DateTime.Today));
        var recurringTransactionWithEndDate = new TestData.RecurringExpense { Recurrence = Recurrence.Daily, EndDate = DateOnly.FromDateTime(DateTime.Today) };
        Context.RegisterRecurringTransaction(recurringTransactionWithEndDate);

        // Act
        await handler.Handle(command: new(), cancellationToken: default);

        // Assert
        await sender.DidNotReceive().Send(Arg.Any<CreateRecurringTransaction.Command>());
    }

    [Fact]
    public async Task CreateRecurrencesOfTheMonthOnTheFirstDay()
    {
        // Arrange
        var firstOfNextMonth = new DateOnly(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1);
        systemDateHelper.TodayDateOnly.Returns(firstOfNextMonth);
        var recurringTransaction1 = new TestData.RecurringExpense { Recurrence = Recurrence.Monthly };
        Context.RegisterRecurringTransaction(recurringTransaction1);

        // Act
        await handler.Handle(command: new(), cancellationToken: default);

        // Assert
        await sender.Received(1).Send(Arg.Any<CreateRecurringTransaction.Command>());
    }
}
