using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MoneyFox.Foundation;

namespace MoneyFox.DataLayer.Entities
{
    public class Account
    {
        //used by EF Core
        private Account(){}

        public Account(string name, double currentBalance = 0, string note = "", bool isExcluded = false)
        {
            UpdateAccount(name, currentBalance, note, isExcluded);
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        [Required]
        public string Name { get; private set; }
        public double CurrentBalance { get; private set; }
        public string Note { get; private set; }
        public bool IsOverdrawn { get; private set; }
        public bool IsExcluded { get; private set; }

        public void UpdateAccount(string name, double currentBalance = 0, string note = "", bool isExcluded = false)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
            CurrentBalance = currentBalance;
            Note = note;
            IsExcluded = isExcluded;
            IsOverdrawn = currentBalance < 0;
        }

        public void AddPaymentAmount(Payment payment)
        {
            ApplyPaymentAmount(payment);
        }

        public void RemovePaymentAmount(Payment payment)
        {
            ApplyPaymentAmount(payment, true);
        }

        private void ApplyPaymentAmount(Payment payment, bool invert = false)
        {
            double amount = invert
                ? -payment.Amount
                : payment.Amount;

            if (payment.Type == PaymentType.Expense
                || payment.Type == PaymentType.Transfer && payment.ChargedAccount.Id == Id)
            {
                CurrentBalance -= amount;
            }
            else
            {
                CurrentBalance += amount;
            }
        }
    }
}