namespace MoneyFox.Core.Aggregates
{

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Common.Interfaces;
    using Dawn;
    using JetBrains.Annotations;
    using Payments;

    public class Account : EntityBase, IAggregateRoot
    {
        [UsedImplicitly]
        private Account() { }

        public Account(string name, decimal initalBalance = 0, string note = "", bool isExcluded = false)
        {
            Guard.Argument(value: name, name: nameof(name)).NotNull().NotWhiteSpace();
            Name = name;
            CurrentBalance = initalBalance;
            Note = note;
            IsExcluded = isExcluded;
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

        public string Name { get; private set; } = null!;

        public decimal CurrentBalance { get; private set; }

        public string? Note { get; private set; }

        public bool IsExcluded { get; private set; }

        public bool IsDeactivated { get; private set; }

        [Obsolete("Will be removed")]
        public bool IsOverdrawn { get; private set; }

        [Obsolete("Will be removed")]
        public DateTime? ModificationDate { get; private set; }

        [Obsolete("Will be removed")]
        public DateTime CreationTime { get; }

        public void UpdateAccount(string name, string note = "", bool isExcluded = false)
        {
            Guard.Argument(value: name, name: nameof(name)).NotNull().NotWhiteSpace();
            Name = name;
            Note = note;
            IsExcluded = isExcluded;
            ModificationDate = DateTime.Now;
        }

        public void AddPaymentAmount(Payment payment)
        {
            if (payment.IsCleared is false)
            {
                return;
            }

            if (payment.Type == PaymentType.Expense || payment.Type == PaymentType.Transfer && payment.ChargedAccount.Id == Id)
            {
                CurrentBalance -= payment.Amount;
            }
            else
            {
                CurrentBalance += payment.Amount;
            }

            ModificationDate = DateTime.Now;
        }

        public void RemovePaymentAmount(Payment payment)
        {
            Guard.Argument(value: payment, name: nameof(payment)).NotNull();
            if (!payment.IsCleared)
            {
                return;
            }

            if (payment.Type == PaymentType.Expense || payment.Type == PaymentType.Transfer && payment.ChargedAccount.Id == Id)
            {
                CurrentBalance -= -payment.Amount;
            }
            else
            {
                CurrentBalance += -payment.Amount;
            }

            ModificationDate = DateTime.Now;
        }

        public void Deactivate()
        {
            IsDeactivated = true;
        }
    }

}
