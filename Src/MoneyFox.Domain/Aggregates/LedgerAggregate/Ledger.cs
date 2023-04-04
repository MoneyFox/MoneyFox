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

    public static Ledger Create(string name, Money currentBalance, string? note = null, bool isExcluded = false)
    {
        return string.IsNullOrWhiteSpace(name)
            ? throw new ArgumentNullException(nameof(name))
            : new(name: name, currentBalance: currentBalance, note: note, isExcludeFromEndOfMonthSummary: isExcluded);
    }
}
