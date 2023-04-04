namespace MoneyFox.Core.Tests.Features.TransactionCreation;

using Core.Common.Interfaces;
using Domain;
using Domain.Aggregates.LedgerAggregate;
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
            reference: testTransaction.Reference,
            type: testTransaction.Type,
            amount: testTransaction.Amount,
            bookingDate: testTransaction.BookingDate,
            categoryId: testTransaction.CategoryId,
            note: testTransaction.Note,
            isTransfer: testTransaction.IsTransfer);

        await handler.Handle(command: command, cancellationToken: CancellationToken.None);

        // Assert
        var dbTransaction = await Context.Transactions.SingleAsync();
    }
}

internal static class CreateTransaction
{
    public record Command : IRequest
    {
        public Command(
            Guid reference,
            TransactionType type,
            Money amount,
            DateOnly bookingDate,
            int? categoryId,
            string? note,
            bool isTransfer)
        {
            Reference = reference;
            Type = type;
            Amount = amount;
            BookingDate = bookingDate;
            CategoryId = categoryId;
            Note = note;
            IsTransfer = isTransfer;
        }

        public Guid Reference { get; }
        public TransactionType Type { get; }
        public Money Amount { get; }
        public DateOnly BookingDate { get; }
        public int? CategoryId { get; }
        public string? Note { get; }
        public bool IsTransfer { get; }
    }

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
