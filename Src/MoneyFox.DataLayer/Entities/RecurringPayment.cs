using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Exceptions;

namespace MoneyFox.DataLayer.Entities
{
    public class RecurringPayment
    {
        private RecurringPayment()
        {
        }

        public RecurringPayment(DateTime startDate,
            double amount,
            PaymentType type,
            PaymentRecurrence recurrence,
            Account chargedAccount,
            string note = "",
            DateTime? endDate = null,
            Account targetAccount = null,
            Category category = null)
        {
            if (!IsEndless && endDate != null && endDate < DateTime.Today)
                throw new MoneyFoxInvalidEndDateException();

            CreationTime = DateTime.Now;
            ChargedAccount = chargedAccount ?? throw new ArgumentNullException(nameof(chargedAccount));
            StartDate = startDate;
            EndDate = endDate;
            Amount = amount;
            Type = type;
            Recurrence = recurrence;
            Note = note;
            Category = category;
            TargetAccount = targetAccount;
            IsEndless = endDate == null;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public bool IsEndless { get; private set; }
        public double Amount { get; private set; }
        public PaymentType Type { get; private set; }
        public PaymentRecurrence Recurrence { get; private set; }
        public string Note { get; set; }

        public DateTime CreationTime { get; private set; }

        public virtual Category Category { get; private set; }

        public virtual Account ChargedAccount { get; private set; }

        public virtual Account TargetAccount { get; private set; }

        public virtual List<Payment> RelatedPayments { get; private set; }

        public void UpdateRecurringPayment(double amount,
            PaymentRecurrence recurrence,
            Account chargedAccount,
            string note = "",
            DateTime? endDate = null,
            Account targetAccount = null,
            Category category = null)
        {
            if (!IsEndless && endDate != null && endDate < DateTime.Today)
                throw new MoneyFoxInvalidEndDateException();

            CreationTime = DateTime.Now;
            ChargedAccount = chargedAccount ?? throw new ArgumentNullException(nameof(chargedAccount));
            EndDate = endDate;
            Amount = amount;
            Recurrence = recurrence;
            Note = note;
            Category = category;
            TargetAccount = targetAccount;
            IsEndless = endDate == null;
        }
    }
}