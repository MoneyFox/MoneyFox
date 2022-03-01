namespace MoneyFox.Core.Aggregates
{
    using Dawn;
    using JetBrains.Annotations;
    using Payments;
    using SharedKernel.Interface;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Account : EntityBase, IAggregateRoot
    {
        [UsedImplicitly]
        private Account() { }

        public Account(string name, decimal initalBalance = 0, string note = "", bool isExcluded = false)
        {
            Guard.Argument(name, nameof(name)).NotNull().NotWhiteSpace();

            Name = name;
            CurrentBalance = initalBalance;
            Note = note;
            IsExcluded = isExcluded;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; [UsedImplicitly] private set; }

        public string Name { get; private set; } = null!;

        public decimal CurrentBalance { get; private set; }

        public string? Note { get; private set; }

        public bool IsExcluded { get; private set; }

        public bool IsDeactivated { get; private set; }

        public void UpdateAccount(string name, string note = "", bool isExcluded = false)
        {
            Guard.Argument(name, nameof(name)).NotNull().NotWhiteSpace();

            Name = name;
            Note = note;
            IsExcluded = isExcluded;
        }

        public void AddPaymentAmount(Payment payment)
        {
            if(payment.IsCleared is false)
            {
                return;
            }

            if(payment.Type == PaymentType.Expense
               || (payment.Type == PaymentType.Transfer && payment.ChargedAccount.Id == Id))
            {
                CurrentBalance -= payment.Amount;
            }
            else
            {
                CurrentBalance += payment.Amount;
            }
        }

        public void RemovePaymentAmount(Payment payment)
        {
            Guard.Argument(payment, nameof(payment)).NotNull();

            if(!payment.IsCleared)
            {
                return;
            }

            if(payment.Type == PaymentType.Expense
               || (payment.Type == PaymentType.Transfer
                   && payment.ChargedAccount.Id == Id))
            {
                CurrentBalance -= -payment.Amount;
            }
            else
            {
                CurrentBalance += -payment.Amount;
            }
        }

        public void Deactivate() => IsDeactivated = true;
    }
}