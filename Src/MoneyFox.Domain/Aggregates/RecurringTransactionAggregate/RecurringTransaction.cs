namespace MoneyFox.Domain.Aggregates.RecurringTransactionAggregate;

using AccountAggregate;

public record struct RecurringTransactionId(int Value);

public sealed class RecurringTransaction : EntityBase
{
    public RecurringTransaction()
    {
        Amount = default!;
    }

    private RecurringTransaction(
        RecurringTransactionId id,
        int chargedAccountId,
        int? targetAccountId,
        Money amount,
        int? categoryId,
        PaymentType type,
        DateOnly startDate,
        DateOnly? endDate,
        Recurrence recurrence,
        string? note,
        bool isLastDayOfMonth,
        DateOnly lastRecurrence)
    {
        Id = id;
        StartDate = startDate;
        EndDate = endDate;
        Amount = amount;
        Type = type;
        Note = note;
        ChargedAccountId = chargedAccountId;
        TargetAccountId = targetAccountId;
        CategoryId = categoryId;
        Recurrence = recurrence;
        IsLastDayOfMonth = isLastDayOfMonth;
        LastRecurrence = lastRecurrence;
    }

    public RecurringTransactionId Id { get; private set; }

    public int ChargedAccountId { get; private set; }

    public int? TargetAccountId { get; private set; }

    public Money Amount { get; private set; }

    public int? CategoryId { get; private set; }

    public PaymentType Type { get; private set; }

    public DateOnly StartDate { get; private set; }

    public DateOnly? EndDate { get; private set; }

    public Recurrence Recurrence { get; private set; }

    public string? Note { get; private set; }

    public bool IsLastDayOfMonth { get; private set; }

    public DateOnly LastRecurrence { get; private set; }

    public static RecurringTransaction Create(
        RecurringTransactionId id,
        int chargedAccount,
        int? targetAccount,
        Money amount,
        int? categoryId,
        PaymentType type,
        DateOnly startDate,
        DateOnly? endDate,
        Recurrence recurrence,
        string? note,
        bool isLastDayOfMonth)
    {
        return new(
            id: id,
            chargedAccountId: chargedAccount,
            targetAccountId: targetAccount,
            amount: amount,
            categoryId: categoryId,
            type: type,
            startDate: startDate,
            endDate: endDate,
            recurrence: recurrence,
            note: note,
            isLastDayOfMonth: isLastDayOfMonth,
            lastRecurrence: DateOnly.FromDateTime(DateTime.Today));
    }
}
