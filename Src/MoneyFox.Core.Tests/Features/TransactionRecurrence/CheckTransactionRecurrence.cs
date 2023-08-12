namespace MoneyFox.Core.Features.TransactionRecurrence;

using Common;
using Common.Interfaces;
using Domain.Aggregates;
using Domain.Tests.TestFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using RecurringTransactionCreation;
using Tests;

public class CheckTransactionRecurrenceHandlerTest : InMemoryTestBase
{
    private readonly CheckTransactionRecurrence.Handler handler;
    private readonly ISender sender;
    private readonly ISystemDateHelper systemDateHelper;

    public CheckTransactionRecurrenceHandlerTest()
    {
        sender = Substitute.For<ISender>();
        systemDateHelper = Substitute.For<ISystemDateHelper>();
        handler = new(dbContext: Context, sender: sender, systemDateHelper: systemDateHelper);
    }

    [Theory]
    [InlineData(Recurrence.Daily)]
    [InlineData(Recurrence.DailyWithoutWeekend)]
    [InlineData(Recurrence.Weekly)]
    [InlineData(Recurrence.Biweekly)]
    [InlineData(Recurrence.Monthly)]
    [InlineData(Recurrence.Bimonthly)]
    [InlineData(Recurrence.Quarterly)]
    [InlineData(Recurrence.Yearly)]
    [InlineData(Recurrence.Biannually)]
    public async Task CreatePaymentIfOriginalPaymentDoesNotExist(Recurrence recurrence)
    {
        // Arrange
        var recurringTransaction = new TestData.RecurringExpense { Recurrence = recurrence };
        Context.RegisterRecurringTransaction(recurringTransaction);

        // Act
        await handler.Handle(command: new(), cancellationToken: default);

        // Assert
        await sender.Received(1).Send(Arg.Any<CreateRecurringTransaction.Command>());
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
        var recurringTransaction3
            = new TestData.RecurringExpense { Recurrence = Recurrence.Daily, EndDate = DateOnly.FromDateTime(DateTime.Today) };
        Context.RegisterRecurringTransactions(recurringTransaction1, recurringTransaction2, recurringTransaction3);

        // Act
        await handler.Handle(command: new(), cancellationToken: default);

        // Assert
        await sender.Received(2).Send(Arg.Any<CreateRecurringTransaction.Command>());
    }
}

internal static class CheckTransactionRecurrence
{
    public record Command : IRequest;

    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly IAppDbContext dbContext;
        private readonly ISender sender;
        private readonly ISystemDateHelper systemDateHelper;

        public Handler(IAppDbContext dbContext, ISender sender, ISystemDateHelper systemDateHelper)
        {
            this.dbContext = dbContext;
            this.sender = sender;
            this.systemDateHelper = systemDateHelper;
        }

        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var recurringTransactions = await dbContext.RecurringTransactions
                .Where(rt => !rt.EndDate.HasValue || rt.EndDate > systemDateHelper.TodayDateOnly)
                .ToListAsync(cancellationToken);
            foreach (var recurringTransaction in recurringTransactions)
            {
                var createRecurringTransactionCommand = new CreateRecurringTransaction.Command(
                    recurringTransactionId: recurringTransaction.RecurringTransactionId,
                    chargedAccount: recurringTransaction.ChargedAccountId,
                    targetAccount: recurringTransaction.TargetAccountId,
                    amount: recurringTransaction.Amount,
                    categoryId: recurringTransaction.CategoryId,
                    startDate: recurringTransaction.StartDate,
                    endDate: recurringTransaction.EndDate,
                    recurrence: recurringTransaction.Recurrence,
                    note: recurringTransaction.Note,
                    isLastDayOfMonth: recurringTransaction.IsLastDayOfMonth,
                    isTransfer: recurringTransaction.IsTransfer);

                var dateAfterRecurrence = recurringTransaction.LastRecurrence;

                while (true)
                {
                    dateAfterRecurrence = DateAfterRecurrence(dateAfterRecurrence: dateAfterRecurrence, recurrence: recurringTransaction.Recurrence);
                    if (dateAfterRecurrence <= systemDateHelper.TodayDateOnly)
                    {
                        await sender.Send(request: createRecurringTransactionCommand, cancellationToken: cancellationToken);
                        continue;
                    }
                    break;
                }
            }
        }

        private DateOnly DateAfterRecurrence(DateOnly dateAfterRecurrence, Recurrence recurrence)
        {
            return recurrence switch
            {
                Recurrence.Daily => dateAfterRecurrence.AddDays(1),
                Recurrence.DailyWithoutWeekend => dateAfterRecurrence.AddDays(1),
                Recurrence.Weekly => dateAfterRecurrence.AddDays(7),
                Recurrence.Biweekly => dateAfterRecurrence.AddDays(14),
                Recurrence.Monthly => dateAfterRecurrence.AddMonths(1),
                Recurrence.Bimonthly => dateAfterRecurrence.AddMonths(2),
                Recurrence.Quarterly => dateAfterRecurrence.AddMonths(3),
                Recurrence.Biannually => dateAfterRecurrence.AddMonths(6),
                Recurrence.Yearly => dateAfterRecurrence.AddYears(1),
                _ => throw new ArgumentOutOfRangeException(nameof(recurrence), recurrence, null)
            };
        }
    }
}
