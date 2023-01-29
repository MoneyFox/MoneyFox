namespace MoneyFox.Domain.Aggregates.LedgerAggregate;

using JetBrains.Annotations;

public record struct TransactionId(int Value);

public enum TransactionType
{
    Expense,
    Income
}

public class Transaction : EntityBase
{

    [UsedImplicitly]
    private Transaction()
    {
        Amount = default!;
    }

    private Transaction(TransactionType type, Money amount, DateOnly bookingDate, int? categoryId, string? note)
    {
        Type = type;
        Amount = amount;
        BookingDate = bookingDate;
        CategoryId = categoryId;
        Note = note;
    }

    public static Transaction Create(TransactionType type, Money amount, DateOnly bookingDate, int? categoryId = null, string? note = null)
    {
        return new(
            type,
            amount,
            bookingDate,
            categoryId,
            note);
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
}
