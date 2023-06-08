namespace MoneyFox.Core.Tests.Features.TransactionCreation;

using Core.Common.Interfaces;
using Domain;
using Domain.Aggregates.LedgerAggregate;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class CreateTransactionTests : InMemoryTestBase
{
    private readonly CreateTransaction.Handler handler;

    public CreateTransactionTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task CreatesTransactionCorrectly()
    {
        // Arrange
        var testTransaction = new TestData.SalaryTransaction();

        // Act
        var command = new CreateTransaction.Command(
            Reference: testTransaction.Reference,
            testTransaction.LedgerId,
            Type: testTransaction.Type,
            Amount: testTransaction.Amount,
            BookingDate: testTransaction.BookingDate,
            CategoryId: testTransaction.CategoryId,
            Note: testTransaction.Note,
            IsTransfer: testTransaction.IsTransfer);

        await handler.Handle(command: command, cancellationToken: CancellationToken.None);

        // Assert
        var dbTransaction = await Context.Transactions.SingleAsync();
        dbTransaction.Id!.Value.Should().BeGreaterThan(0);
        dbTransaction.Reference.Should().Be(testTransaction.Reference);
        dbTransaction.Type.Should().Be(testTransaction.Type);
        dbTransaction.Amount.Should().Be(testTransaction.Amount);
        dbTransaction.BookingDate.Should().Be(testTransaction.BookingDate);
        dbTransaction.CategoryId.Should().Be(testTransaction.CategoryId);
        dbTransaction.Note.Should().Be(testTransaction.Note);
        dbTransaction.IsTransfer.Should().Be(testTransaction.IsTransfer);
    }
}

internal static class CreateTransaction
{
    public record Command(
        Guid Reference,
        LedgerId LedgerId,
        TransactionType Type,
        Money Amount,
        DateOnly BookingDate,
        int? CategoryId,
        string? Note,
        bool IsTransfer) : IRequest;

    public class Handler : IRequestHandler<Command>
    {
        private readonly IAppDbContext dbContext;

        public Handler(IAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var transaction = Transaction.Create(
                reference: command.Reference,
                ledgerId: command.LedgerId,
                type: command.Type,
                amount: command.Amount,
                bookingDate: command.BookingDate,
                categoryId: command.CategoryId,
                note: command.Note,
                isTransfer: command.IsTransfer);

            await dbContext.AddAsync(entity: transaction, cancellationToken: cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
