namespace MoneyFox.Domain.Aggregates.RecurringTransactionAggregate;

using JetBrains.Annotations;

public record struct RecurringTransactionId(int Value);

public sealed class RecurringTransaction : EntityBase
{
    public RecurringTransaction()
    {
        Amount = default!;
    }

    private RecurringTransaction(
        Guid recurringTransactionId,
        int chargedAccountId,
        int? targetAccountId,
        Money amount,
        int? categoryId,
        DateOnly startDate,
        DateOnly? endDate,
        Recurrence recurrence,
        string? note,
        bool isLastDayOfMonth,
        DateOnly lastRecurrence,
        bool isTransfer)
    {
        RecurringTransactionId = recurringTransactionId;
        StartDate = startDate;
        EndDate = endDate;
        Amount = amount;
        Note = note;
        ChargedAccountId = chargedAccountId;
        TargetAccountId = targetAccountId;
        CategoryId = categoryId;
        Recurrence = recurrence;
        IsLastDayOfMonth = isLastDayOfMonth;
        LastRecurrence = lastRecurrence;
        IsTransfer = isTransfer;
    }

    public RecurringTransactionId Id
    {
        get;

        [UsedImplicitly]
        private set;
    }

    public Guid RecurringTransactionId { get; private set; }

    public int ChargedAccountId { get; private set; }

    public int? TargetAccountId { get; private set; }

    public Money Amount { get; private set; }

    public int? CategoryId { get; private set; }

    public DateOnly StartDate { get; private set; }

    public DateOnly? EndDate { get; private set; }

    public Recurrence Recurrence { get; private set; }

    public string? Note { get; private set; }

    public bool IsLastDayOfMonth { get; private set; }

    public bool IsTransfer { get; private set; }

    public DateOnly LastRecurrence { get; private set; }

    public static RecurringTransaction Create(
        Guid recurringTransactionId,
        int chargedAccount,
        int? targetAccount,
        Money amount,
        int? categoryId,
        DateOnly startDate,
        DateOnly? endDate,
        Recurrence recurrence,
        string? note,
        bool isLastDayOfMonth,
        bool isTransfer,
        DateOnly lastRecurrence)
    {
        return new(
            recurringTransactionId: recurringTransactionId,
            chargedAccountId: chargedAccount,
            targetAccountId: targetAccount,
            amount: amount,
            categoryId: categoryId,
            startDate: startDate,
            endDate: endDate,
            recurrence: recurrence,
            note: note,
            isLastDayOfMonth: isLastDayOfMonth,
            lastRecurrence: lastRecurrence,
            isTransfer: isTransfer);
    }

    public void UpdateRecurrence(
        Money updatedAmount,
        int? updatedCategoryId,
        Recurrence updatedRecurrence,
        DateOnly? updatedEndDate,
        bool updatedIsLastDayOfMonth)
    {
        Amount = updatedAmount;
        CategoryId = updatedCategoryId;
        Recurrence = updatedRecurrence;
        EndDate = updatedEndDate;
        IsLastDayOfMonth = updatedIsLastDayOfMonth;
    }

    public void EndRecurrence()
    {
        EndDate = DateOnly.FromDateTime(DateTime.Today);
    }
}
