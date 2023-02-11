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
        LedgerBalance = default!;
    }

    private Transaction(
        Guid reference,
        TransactionType type,
        Money amount,
        Money ledgerBalance,
        DateOnly bookingDate,
        int? categoryId,
        string? note,
        bool isTransfer)
    {
        Reference = reference;
        Type = type;
        Amount = amount;
        LedgerBalance = ledgerBalance;
        BookingDate = bookingDate;
        CategoryId = categoryId;
        Note = note;
        IsTransfer = isTransfer;
    }

    public TransactionId Id
    {
        get;

        [UsedImplicitly]
        private set;
    }

    public Guid Reference
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

    public Money LedgerBalance
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

    public bool IsTransfer
    {
        get;

        [UsedImplicitly]
        private set;
    }

    public static Transaction Create(
        Guid reference,
        TransactionType type,
        Money amount,
        Money ledgerBalance,
        DateOnly bookingDate,
        int? categoryId = null,
        string? note = null,
        bool isTransfer = false)
    {
        return type switch
        {
            TransactionType.Income when amount.Amount < 0 => throw new InvalidTransactionAmountException("Income has to have a a positive amount."),
            TransactionType.Expense when amount.Amount > 0 => throw new InvalidTransactionAmountException("Expense has to have a a negative amount."),
            _ => new(
                reference: reference,
                type: type,
                amount: amount,
                ledgerBalance: ledgerBalance,
                bookingDate: bookingDate,
                categoryId: categoryId,
                note: note,
                isTransfer: isTransfer)
        };
    }
}
