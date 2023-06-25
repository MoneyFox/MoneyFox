namespace MoneyFox.Domain.Aggregates.LedgerAggregate;

using JetBrains.Annotations;

public record struct LedgerId(int Value);

public class Ledger : EntityBase
{
    [UsedImplicitly]
    private Ledger()
    {
        CurrentBalance = default!;
        OpeningBalance = default!;
    }

    private Ledger(
        string name,
        Money openingBalance,
        Money currentBalance,
        string? note,
        bool isExcludeFromEndOfMonthSummary)
    {
        Name = name;
        OpeningBalance = openingBalance;
        Note = note;
        IsExcludeFromEndOfMonthSummary = isExcludeFromEndOfMonthSummary;
        CurrentBalance = currentBalance;
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

    public Currency Currency
    {
        get;

        [UsedImplicitly]
        private set;
    }

    public decimal OpeningBalance
    {
        get;

        [UsedImplicitly]
        private set;
    }

    public decimal CurrentBalance
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

    public static Ledger Create(string name, Money openingBalance, string? note = null, bool isExcluded = false)
    {
        return string.IsNullOrWhiteSpace(name)
            ? throw new ArgumentNullException(nameof(name))
            : new(
                name: name,
                openingBalance: openingBalance,
                currentBalance: openingBalance,
                note: note,
                isExcludeFromEndOfMonthSummary: isExcluded);
    }

    public void Deposit(Money amount)
    {
        CurrentBalance -= amount;
    }

    public void Withdraw(Money amount)
    {
        CurrentBalance += amount;
    }
}
