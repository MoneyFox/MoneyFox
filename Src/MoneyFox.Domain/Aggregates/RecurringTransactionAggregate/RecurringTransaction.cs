namespace MoneyFox.Domain.Aggregates.RecurringTransactionAggregate;

using AccountAggregate;

public record struct RecurringTransactionId(int Value);

public sealed class RecurringTransaction : EntityBase
{
    private RecurringTransaction(
        RecurringTransactionId id,
        DateOnly startDate,
        DateOnly? endDate,
        Money amount,
        PaymentType type,
        string? note,
        int chargedAccount,
        int? targetAccount,
        int? categoryId,
        Recurrence recurrence,
        bool isLastDayOfMonth,
        DateOnly lastRecurrence)
    {
        Id = id;
        StartDate = startDate;
        EndDate = endDate;
        Amount = amount;
        Type = type;
        Note = note;
        ChargedAccount = chargedAccount;
        TargetAccount = targetAccount;
        CategoryId = categoryId;
        Recurrence = recurrence;
        IsLastDayOfMonth = isLastDayOfMonth;
        LastRecurrence = lastRecurrence;
    }

    public RecurringTransactionId Id { get; private set; }

    public DateOnly StartDate { get; private set; }

    public DateOnly? EndDate { get; private set; }

    public Money Amount { get; private set; }

    public PaymentType Type { get; private set; }

    public string? Note { get; private set; }

    public int ChargedAccount { get; private set; }

    public int? TargetAccount { get; private set; }

    public int? CategoryId { get; private set; }

    public Recurrence Recurrence { get; private set; }

    public bool IsLastDayOfMonth { get; private set; }

    public DateOnly LastRecurrence { get; private set; }

    public static RecurringTransaction Create(
        RecurringTransactionId id,
        DateOnly startDate,
        DateOnly? endDate,
        Money amount,
        PaymentType type,
        string? note,
        int chargedAccount,
        int? targetAccount,
        int? categoryId,
        Recurrence recurrence,
        bool isLastDayOfMonth)
    {
        return new(
            id: id,
            startDate: startDate,
            endDate: endDate,
            amount: amount,
            type: type,
            note: note,
            chargedAccount: chargedAccount,
            targetAccount: targetAccount,
            categoryId: categoryId,
            recurrence: recurrence,
            isLastDayOfMonth: isLastDayOfMonth,
            lastRecurrence: DateOnly.FromDateTime(DateTime.Today));
    }
}
