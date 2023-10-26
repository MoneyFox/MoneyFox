namespace MoneyFox.Domain.Aggregates.AccountAggregate;

using CategoryAggregate;
using Exceptions;
using JetBrains.Annotations;

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
        string? note = "",
        Guid? recurringTransactionId = null)
    {
        Date = date;
        Amount = amount;
        Type = type;
        Note = note;
        ChargedAccount = chargedAccount ?? throw new AccountNullException();
        TargetAccount = type == PaymentType.Transfer ? targetAccount : null;
        Category = category;
        RecurringTransactionId = recurringTransactionId;
        ClearPayment();
        if (RecurringTransactionId.HasValue)
        {
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

    [Obsolete("To be removed")]
    public virtual RecurringPayment? RecurringPayment { get; } = null!;

    public Guid? RecurringTransactionId { get; private set; }

    public void UpdatePayment(
        DateTime date,
        decimal amount,
        PaymentType type,
        Account chargedAccount,
        Account? targetAccount = null,
        Category? category = null,
        string? note = "")
    {
        if (ChargedAccount == null)
        {
            throw new InvalidOperationException("Uninitialized " + nameof(ChargedAccount));
        }

        ChargedAccount.RemovePaymentAmount(this);
        TargetAccount?.RemovePaymentAmount(this);
        Date = date;
        Amount = amount;
        Type = type;
        Note = note;
        ChargedAccount = chargedAccount ?? throw new AccountNullException();
        TargetAccount = type == PaymentType.Transfer ? targetAccount : null;
        Category = category;
        ClearPayment();
    }

    public void AddRecurringTransaction(Guid recurringTransactionId)
    {
        RecurringTransactionId = recurringTransactionId;
        IsRecurring = true;
    }

    public void ClearPayment()
    {
        IsCleared = Date.Date <= DateTime.Today.Date;
        if (ChargedAccount == null)
        {
            throw new InvalidOperationException("Uninitialized " + nameof(ChargedAccount));
        }

        ChargedAccount.AddPaymentAmount(this);
        if (Type == PaymentType.Transfer)
        {
            if (TargetAccount == null)
            {
                return;
            }

            TargetAccount.AddPaymentAmount(this);
        }
    }

    public void RemoveCategory()
    {
        Category = null;
    }
}
