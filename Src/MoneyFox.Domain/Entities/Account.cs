using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyFox.Domain.Entities
{
    public class Account
    {
        //used by EF Core
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        private Account()
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {
        }

        public Account(string name, decimal currentBalance = 0, string note = "", bool isExcluded = false)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            ModificationDate = DateTime.Now;
            CreationTime = DateTime.Now;

            Name = name;
            CurrentBalance = currentBalance;
            Note = note;
            IsExcluded = isExcluded;
            IsOverdrawn = currentBalance < 0;
            ModificationDate = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        [Required] public string Name { get; private set; }

        public decimal CurrentBalance { get; private set; }

        public string? Note { get; set; }

        public bool IsOverdrawn { get; private set; }

        public bool IsExcluded { get; private set; }

        public DateTime ModificationDate { get; private set; }

        public DateTime CreationTime { get; private set; }

        public void UpdateAccount(string name, decimal currentBalance = 0m, string note = "", bool isExcluded = false)
        {
            if(string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
            CurrentBalance = currentBalance;
            Note = note;
            IsExcluded = isExcluded;
            IsOverdrawn = currentBalance < 0;

            ModificationDate = DateTime.Now;
        }

        public void AddPaymentAmount(Payment payment)
        {
            ThrowIfPaymentNull(payment);
            ApplyPaymentAmount(payment);
        }

        public void RemovePaymentAmount(Payment payment)
        {
            ThrowIfPaymentNull(payment);
            ApplyPaymentAmount(payment, true);
        }

        private void ApplyPaymentAmount(Payment payment, bool invert = false)
        {
            if (payment.ChargedAccount == null) throw new InvalidOperationException("Uninitialized property: " + nameof(payment.ChargedAccount));
            if (!payment.IsCleared) return;

            decimal amount = invert
                             ? -payment.Amount
                             : payment.Amount;

            if(payment.Type == PaymentType.Expense
               || payment.Type == PaymentType.Transfer
               && payment.ChargedAccount.Id == Id)
            {
                CurrentBalance -= amount;
            }
            else
            {
                CurrentBalance += amount;
            }
            ModificationDate = DateTime.Now;
        }

        private static void ThrowIfPaymentNull(Payment payment)
        {
            if(payment == null)
                throw new ArgumentNullException(nameof(payment));
        }
    }
}
