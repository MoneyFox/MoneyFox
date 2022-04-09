namespace MoneyFox.Core.Aggregates.Payments
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using _Pending_.Exceptions;
    using JetBrains.Annotations;

    public class RecurringPayment : EntityBase
    {
        [UsedImplicitly]
        private RecurringPayment() { }

        public RecurringPayment(
            DateTime startDate,
            decimal amount,
            PaymentType type,
            PaymentRecurrence recurrence,
            Account chargedAccount,
            string note = "",
            DateTime? endDate = null,
            Account? targetAccount = null,
            Category? category = null,
            DateTime? lastRecurrenceCreated = null)
        {
            if (!IsEndless && endDate != null && endDate < DateTime.Today)
            {
                throw new InvalidEndDateException();
            }

            ChargedAccount = chargedAccount ?? throw new ArgumentNullException(nameof(chargedAccount));
            StartDate = startDate;
            EndDate = endDate;
            Amount = amount;
            Type = type;
            Recurrence = recurrence;
            Note = note;
            Category = category;
            TargetAccount = type == PaymentType.Transfer ? targetAccount : null;
            IsEndless = endDate == null;
            LastRecurrenceCreated = lastRecurrenceCreated ?? DateTime.Now;
            CreationTime = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id
        {
            get;

            [UsedImplicitly]
            private set;
        }

        public DateTime StartDate
        {
            get;

            [UsedImplicitly]
            private set;
        }

        public DateTime? EndDate { get; private set; }

        public bool IsEndless { get; private set; }

        public decimal Amount { get; private set; }

        public PaymentType Type
        {
            get;

            [UsedImplicitly]
            private set;
        }

        public PaymentRecurrence Recurrence { get; private set; }

        public string? Note
        {
            get;

            [UsedImplicitly]
            private set;
        }

        public DateTime LastRecurrenceCreated { get; private set; }

        public Category? Category { get; private set; }

        public Account ChargedAccount { get; private set; }

        public Account? TargetAccount { get; private set; }

        public virtual List<Payment> RelatedPayments
        {
            get;

            [UsedImplicitly]
            private set;
        } = new List<Payment>();

        [Obsolete("Will be removed")]
        public DateTime? ModificationDate { get; private set; }

        [Obsolete("Will be removed")]
        public DateTime CreationTime
        {
            get;

            [UsedImplicitly]
            private set;
        }

        public void UpdateRecurringPayment(
            decimal amount,
            PaymentRecurrence recurrence,
            Account chargedAccount,
            string note = "",
            DateTime? endDate = null,
            Account? targetAccount = null,
            Category? category = null)
        {
            if (!IsEndless && endDate != null && endDate < DateTime.Today)
            {
                throw new InvalidEndDateException();
            }

            ChargedAccount = chargedAccount ?? throw new ArgumentNullException(nameof(chargedAccount));
            EndDate = endDate;
            Amount = amount;
            Recurrence = recurrence;
            Note = note;
            Category = category;
            TargetAccount = Type == PaymentType.Transfer ? targetAccount : null;
            IsEndless = endDate == null;
            ModificationDate = DateTime.Now;
        }

        public void SetLastRecurrenceCreatedDate()
        {
            LastRecurrenceCreated = DateTime.Now;
            ModificationDate = DateTime.Now;
        }
    }

}
