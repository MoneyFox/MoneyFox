namespace MoneyFox.Domain.Aggregates.LedgerAggregate;

using System.Collections.Immutable;
using JetBrains.Annotations;

public record struct LedgerId(int Value);

public class Ledger : EntityBase
{
    [UsedImplicitly]
    private Ledger()
    {
        OpeningBalance = default!;
    }

    private Ledger(string name, Money openingBalance, string? note, bool isExcludeFromEndOfMonthSummary)
    {
        Name = name;
        OpeningBalance = openingBalance;
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

    public Money OpeningBalance
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
            : new(name: name, openingBalance: currentBalance, note: note, isExcludeFromEndOfMonthSummary: isExcluded);
    }
}
