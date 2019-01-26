using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MoneyFox.Foundation;

namespace MoneyFox.DataLayer.Entities
{
    public class RecurringPayment
    {
        private RecurringPayment() { }

        public RecurringPayment(DateTime startDate, 
            double amount,
            PaymentType type,
            PaymentRecurrence recurrence,
            string note, 
            Account chargedAccount, 
            DateTime? endDate = null,
            Account targetAccount = null, 
            Category category = null)
        {
            ChargedAccount = chargedAccount;
            StartDate = startDate;
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

        public virtual Category Category { get; set; }

        public virtual Account ChargedAccount { get; set; }

        public virtual Account TargetAccount { get; set; }

        //public virtual List<Payment> RelatedPayments { get; set; }
    }
}