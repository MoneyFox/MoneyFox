namespace MoneyFox.Domain.Aggregates.LedgerAggregate;

using System.Collections.Immutable;
using JetBrains.Annotations;

public record struct LedgerId(int Value);

public class Ledger : EntityBase
{
    [UsedImplicitly]
    private Ledger()
    {
        CurrentBalance = default!;
    }

    private Ledger(string name, Money currentBalance, string? note, bool isExcludeFromEndOfMonthSummary)
    {
        Name = name;
        CurrentBalance = currentBalance;
        Note = note;
        IsExcludeFromEndOfMonthSummary = isExcludeFromEndOfMonthSummary;
    }

    public LedgerId Id
    {
        get;

        [UsedImplicitly]
        private set;
    }

    public string Name
    {
        get;

        [UsedImplicitly]
        private set;
    } = null!;

    public Money CurrentBalance
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

    public bool IsExcludeFromEndOfMonthSummary
    {
        get;

        [UsedImplicitly]
        private set;
    }

    public ImmutableList<Transaction> Transactions
    {
        get;

        [UsedImplicitly]
        private set;
    } = ImmutableList<Transaction>.Empty;

    public static Ledger Create(string name, Money currentBalance, string? note = null, bool isExcluded = false)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        return new(name: name, currentBalance: currentBalance, note: note, isExcludeFromEndOfMonthSummary: isExcluded);
    }

    public void AddTransaction(
        Guid reference,
        TransactionType type,
        Money amount,
        DateOnly bookingDate,
        int? categoryId,
        string? note)
    {
        CurrentBalance = new(amount: CurrentBalance + amount, currency: CurrentBalance.Currency);
        var transaction = Transaction.Create(
            reference: reference,
            type: type,
            amount: amount,
            ledgerBalance: CurrentBalance,
            bookingDate: bookingDate,
            categoryId: categoryId,
            note: note);

        Transactions = Transactions.Add(transaction);
    }
}
