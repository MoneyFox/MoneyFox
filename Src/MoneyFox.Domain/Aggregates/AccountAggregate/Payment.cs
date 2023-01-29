namespace MoneyFox.Domain.Aggregates.AccountAggregate;

using CategoryAggregate;
using Exceptions;
using JetBrains.Annotations;
using Serilog;

public class Payment : EntityBase
{
    [UsedImplicitly]
    private Payment() { }

    public Payment(
        DateTime date,
        decimal amount,
        PaymentType type,
        Account chargedAccount,
        Account? targetAccount = null,
        Category? category = null,
        string note = "",
        RecurringPayment? recurringPayment = null)
    {
        AssignValues(
            date: date,
            amount: amount,
            type: type,
            chargedAccount: chargedAccount,
            targetAccount: targetAccount,
            category: category,
            note: note);

        ClearPayment();
        if (recurringPayment != null)
        {
            RecurringPayment = recurringPayment;
            IsRecurring = true;
        }
    }

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

    public DateTime Date { get; private set; }

    public decimal Amount { get; private set; }

    public bool IsCleared { get; private set; }

    public PaymentType Type { get; private set; }

    public string? Note { get; private set; }

    public bool IsRecurring { get; private set; }

    public virtual Category? Category { get; private set; }

    public virtual Account ChargedAccount { get; private set; } = null!;

    public virtual Account? TargetAccount { get; private set; }

    public virtual RecurringPayment? RecurringPayment { get; private set; }

    public void UpdatePayment(
        DateTime date,
        decimal amount,
        PaymentType type,
        Account chargedAccount,
        Account? targetAccount = null,
        Category? category = null,
        string note = "")
    {
        if (ChargedAccount == null)
        {
            throw new InvalidOperationException("Uninitialized property: " + nameof(ChargedAccount));
        }

        ChargedAccount.RemovePaymentAmount(this);
        TargetAccount?.RemovePaymentAmount(this);
        AssignValues(
            date: date,
            amount: amount,
            type: type,
            chargedAccount: chargedAccount,
            targetAccount: targetAccount,
            category: category,
            note: note);

        ClearPayment();
    }

    private void AssignValues(
        DateTime date,
        decimal amount,
        PaymentType type,
        Account chargedAccount,
        Account? targetAccount,
        Category? category,
        string note)
    {
        Date = date;
        Amount = amount;
        Type = type;
        Note = note;
        ChargedAccount = chargedAccount ?? throw new AccountNullException();
        TargetAccount = type == PaymentType.Transfer ? targetAccount : null;
        Category = category;
    }

    public void AddRecurringPayment(PaymentRecurrence recurrence, bool isLastDayOfMonth = false, DateTime? endDate = null)
    {
        RecurringPayment = new(
            startDate: Date,
            amount: Amount,
            type: Type,
            recurrence: recurrence,
            chargedAccount: ChargedAccount,
            isLastDayOfMonth: isLastDayOfMonth,
            note: Note ?? "",
            endDate: endDate,
            targetAccount: TargetAccount,
            category: Category,
            lastRecurrenceCreated: Date);

        IsRecurring = true;
    }

    public void RemoveRecurringPayment()
    {
        RecurringPayment = null;
        IsRecurring = false;
    }

    public void ClearPayment()
    {
        IsCleared = Date.Date <= DateTime.Today.Date;
        if (ChargedAccount == null)
        {
            throw new InvalidOperationException("Uninitialized property: " + nameof(ChargedAccount));
        }

        ChargedAccount.AddPaymentAmount(this);
        if (Type == PaymentType.Transfer)
        {
            if (TargetAccount == null)
            {
                Log.Warning(messageTemplate: "Target Account on clearing was null for payment {id}", propertyValue: Id);

                return;
            }

            TargetAccount.AddPaymentAmount(this);
        }
    }
}
