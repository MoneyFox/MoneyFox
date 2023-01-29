namespace MoneyFox.Domain.Aggregates.LedgerAggregate;

using JetBrains.Annotations;

public record struct TransactionId(int Value);
public enum TransactionType { Expense, Income }

public class InvalidTransactionAmountException : Exception
{
    public InvalidTransactionAmountException(string message) : base(message: message) { }
}

public class Transaction : EntityBase
{
    [UsedImplicitly]
    private Transaction()
    {
        Amount = default!;
    }

    private Transaction(
        TransactionType type,
        Money amount,
        DateOnly bookingDate,
        int? categoryId,
        string? note)
    {
        Type = type;
        Amount = amount;
        BookingDate = bookingDate;
        CategoryId = categoryId;
        Note = note;
    }

    public TransactionId Id
    {
        get;

        [UsedImplicitly]
        private set;
    }

    public TransactionType Type
    {
        get;

        [UsedImplicitly]
        private set;
    }

    public Money Amount
    {
        get;

        [UsedImplicitly]
        private set;
    }

    public DateOnly BookingDate
    {
        get;

        [UsedImplicitly]
        private set;
    }

    public int? CategoryId
    {
        get;

        [UsedImplicitly]
        private set;
    }

    public string? Note
    {
        get;

        [UsedImplicitly]
        private set;
    }

    public static Transaction Create(
        TransactionType type,
        Money amount,
        DateOnly bookingDate,
        int? categoryId = null,
        string? note = null)
    {
        if (type == TransactionType.Income && amount.Amount < 0)
        {
            throw new InvalidTransactionAmountException("Income has to have a a positive amount.");
        }
        if (type == TransactionType.Expense && amount.Amount > 0)
        {
            throw new InvalidTransactionAmountException("Expense has to have a a negative amount.");
        }

        return new(
            type: type,
            amount: amount,
            bookingDate: bookingDate,
            categoryId: categoryId,
            note: note);
    }
}
