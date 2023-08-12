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
        systemDateHelper.Today.Returns(DateTime.Today.AddDays(2));
        var recurringTransaction = new TestData.RecurringExpense { Recurrence = Recurrence.Daily };
        Context.RegisterRecurringTransaction(recurringTransaction);

        // Act
        await handler.Handle(command: new(), cancellationToken: default);

        // Assert
        await sender.Received(2).Send(Arg.Any<CreateRecurringTransaction.Command>());
    }

    // [Theory]
    // [InlineData(PaymentRecurrence.Daily, 0)]
    // [InlineData(PaymentRecurrence.Weekly, 5)]
    // [InlineData(PaymentRecurrence.Biweekly, 10)]
    // [InlineData(PaymentRecurrence.Monthly, 28)]
    // [InlineData(PaymentRecurrence.Bimonthly, 55)]
    // [InlineData(PaymentRecurrence.Quarterly, 88)]
    // [InlineData(PaymentRecurrence.Biannually, 180)]
    // [InlineData(PaymentRecurrence.Yearly, 340)]
    // public async Task Foo(PaymentRecurrence recurrence, int days)
    // {
    //     // Arrange
    //     var recurringTransaction = new TestData.RecurringExpense();
    //     Context.RegisterRecurringTransaction(recurringTransaction);
    //
    //     // Act
    //     await new CheckTransactionRecurrence.Handler().Handle(command: new(), cancellationToken: default);
    //
    //     // Assert
    //     Context.Payments.Should().ContainSingle();
    // }
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
            var recurringTransaction = await dbContext.RecurringTransactions.SingleAsync(cancellationToken);
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

            var dateDiff = systemDateHelper.Today - recurringTransaction.LastRecurrence.ToDateTime(TimeOnly.MinValue);
            for (var i = 0; i < dateDiff.TotalDays; i++)
            {
                await sender.Send(request: createRecurringTransactionCommand, cancellationToken: cancellationToken);
            }
        }
    }
}
