namespace MoneyFox.Domain.Aggregates;

using AccountAggregate;
using JetBrains.Annotations;

internal sealed class RecurringTransaction : EntityBase
{
    public int Id
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

    public DateOnly StartDate { get; private set; }

    public DateOnly EndDate { get; private set; }

    public decimal Amount { get; private set; }

    public PaymentType Type { get; private set; }

    public string? Note { get; private set; }

    public int ChargedAccount { get; private set; }

    public int? TargetAccount { get; private set; }

    public int? Category { get; private set; }

    public Recurrence Recurrence{ get; private set; }

    public bool IsLastDayOfMonth { get; private set; }

    public DateOnly LastRecurrence {get; private set; }

}
