using Dawn;
using JetBrains.Annotations;
using MoneyFox.Core.Aggregates.Payments;
using MoneyFox.SharedKernel.Interface;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyFox.Core.Aggregates
{
    public class Account : IAggregateRoot
    {
        [UsedImplicitly]
        private Account() { }

        public Account(string name, decimal currentBalance = 0, string note = "", bool isExcluded = false)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

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
        public int Id { get; [UsedImplicitly] private set; }

        /// <summary>
        ///     The name of the account.
        /// </summary>
        [Required]
        public string Name { get; private set; } = null!;

        /// <summary>
        ///     The current account balance.
        /// </summary>
        public decimal CurrentBalance { get; private set; }

        /// <summary>
        ///     A note to this account.
        /// </summary>
        public string? Note { get; private set; }

        // todo remove this and make calculated property.
        /// <summary>
        ///     Indicates if this account is overdrawn or not.
        /// </summary>
        public bool IsOverdrawn { get; private set; }

        /// <summary>
        ///     Indicates that this Account should not be included in end of month calculations.
        /// </summary>
        public bool IsExcluded { get; private set; }

        /// <summary>
        ///     Indicates that an account is disabled and should no longer be displayed
        /// </summary>
        public bool IsDeactivated { get; private set; }

        /// <summary>
        ///     Date of the last modification
        /// </summary>
        public DateTime ModificationDate { get; private set; }

        /// <summary>
        ///     Date when the entry was created.
        /// </summary>
        public DateTime CreationTime { get; }

        public void UpdateAccount(string name, decimal currentBalance = 0m, string note = "", bool isExcluded = false)
        {
            Guard.Argument(name, nameof(name)).NotNull().NotWhiteSpace();

            Name = name;
            CurrentBalance = currentBalance;
            Note = note;
            IsExcluded = isExcluded;
            IsOverdrawn = currentBalance < 0;

            ModificationDate = DateTime.Now;
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

            ModificationDate = DateTime.Now;
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

            ModificationDate = DateTime.Now;
        }

        public void Deactivate() => IsDeactivated = true;
    }
}