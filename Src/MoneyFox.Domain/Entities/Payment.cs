using System;
using System.ComponentModel.DataAnnotations;
using MoneyFox.Domain.Exceptions;

namespace MoneyFox.Domain.Entities
{
    /// <summary>
    ///     Database model for payments. Includes expenses, income and transfers.
    ///     Database table: Payments
    /// </summary>
    public class Payment
    {
        private Payment()
        {
        }

        public Payment(DateTime date,
                       double amount,
                       PaymentType type,
                       Account chargedAccount,
                       Account targetAccount = null,
                       Category category = null,
                       string note = "",
                       RecurringPayment recurringPayment = null)
        {
            CreationTime = DateTime.Now;
            AssignValues(date, amount, type, chargedAccount, targetAccount, category, note);

            ClearPayment();

            if (recurringPayment != null)
            {
                RecurringPayment = recurringPayment;
                IsRecurring = true;
            }
        }

        public int Id { get; private set; }

        public int? CategoryId { get; private set; }

        public DateTime Date { get; private set; }
        public double Amount { get; private set; }
        public bool IsCleared { get; private set; }
        public PaymentType Type { get; private set; }
        public string Note { get; private set; }
        public bool IsRecurring { get; private set; }

        public DateTime ModificationDate { get; private set; }
        public DateTime CreationTime { get; private set; }

        public virtual Category Category { get; private set; }

        [Required]
        public virtual Account ChargedAccount { get; private set; }

        public virtual Account TargetAccount { get; private set; }

        public virtual RecurringPayment RecurringPayment { get; private set; }

        public void UpdatePayment(DateTime date,
                                  double amount,
                                  PaymentType type,
                                  Account chargedAccount,
                                  Account targetAccount = null,
                                  Category category = null,
                                  string note = "")
        {
            ChargedAccount.RemovePaymentAmount(this);
            TargetAccount?.RemovePaymentAmount(this);

            AssignValues(date, amount, type, chargedAccount, targetAccount, category, note);

            ClearPayment();
        }

        private void AssignValues(DateTime date, double amount, PaymentType type, Account chargedAccount, Account targetAccount,
                                  Category category, string note)
        {
            Date = date;
            Amount = amount;
            Type = type;
            Note = note;
            ChargedAccount = chargedAccount ?? throw new AccountNullException();
            TargetAccount = targetAccount;
            Category = category;
            ModificationDate = DateTime.Now;
        }

        public void AddRecurringPayment(PaymentRecurrence recurrence, DateTime? endDate)
        {
            RecurringPayment = new RecurringPayment(Date, Amount, Type, recurrence, ChargedAccount, Note, endDate, TargetAccount, Category);
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
            ChargedAccount.AddPaymentAmount(this);

            if (Type == PaymentType.Transfer)
            {
                TargetAccount.AddPaymentAmount(this);
            }
        }
    }
}
