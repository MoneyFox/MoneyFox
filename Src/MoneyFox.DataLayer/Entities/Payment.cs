using System;
using System.ComponentModel.DataAnnotations;
using MoneyFox.Foundation;

namespace MoneyFox.DataLayer.Entities
{
    /// <summary>
    ///     Databasemodel for payments. Includes expenses, income and transfers.
    ///     Databasetable: Payments
    /// </summary>
    public class Payment
    {
        private Payment() { }

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
            UpdatePayment(date, amount, type, chargedAccount, targetAccount, category, note);

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
            Date = date;
            Amount = amount;
            Type = type;
            Note = note;
            ChargedAccount = chargedAccount ?? throw new ArgumentNullException(nameof(chargedAccount));
            TargetAccount = targetAccount;
            Category = category;

            ClearPayment();
        }

        public void AddRecurringPayment(PaymentRecurrence recurrence, DateTime? endDate)
        {
            RecurringPayment = new RecurringPayment(Date, Amount, Type, recurrence, ChargedAccount, Note, endDate, TargetAccount, Category);
            IsRecurring = true;
        }

        public void ClearPayment()
        {
            IsCleared = Date.Date <= DateTime.Today.Date;
        }
    }
}