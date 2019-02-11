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
        private RecurringPayment() { }

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

        [Required]
        public int ChargedAccountId { get; set; }

        public int? TargetAccountId { get; set; }

        public int? CategoryId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsEndless { get; set; }
        public double Amount { get; set; }
        public PaymentType Type { get; set; }
        public PaymentRecurrence Recurrence { get; set; }
        public string Note { get; set; }

        public DateTime CreationTime { get; private set; }

        public virtual Category Category { get; set; }

        public virtual Account ChargedAccount { get; set; }

        public virtual Account TargetAccount { get; set; }

        public virtual List<Payment> RelatedPayments { get; set; }
    }
}