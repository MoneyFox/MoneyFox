using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PropertyChanged;

namespace MoneyFox.Core.DatabaseModels
{
    /// <summary>
    ///     Databasemodel for payments. Includes expenses, income and transfers.
    ///     Databasetable: Payments
    /// </summary>
    [Table("Payments")]
    [ImplementPropertyChanged]
    public class Payment
    {
        public int Id { get; set; }

        [Required]
        public int ChargedAccountId { get; set; }

        public int? TargetAccountId { get; set; }

        public int? CategoryId { get; set; }

        public DateTime Date { get; set; }

        public double Amount { get; set; }

        public bool IsCleared { get; set; }

        [Required]
        public int Type { get; set; }

        public string Note { get; set; }

        public bool IsRecurring { get; set; }

        public int RecurringPaymentId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category;

        [ForeignKey(nameof(ChargedAccountId))]
        public Account ChargedAccount;

        [ForeignKey(nameof(TargetAccountId))]
        public Account TargetAccount;

        [ForeignKey(nameof(RecurringPaymentId))]
        public RecurringPayment RecurringPayment;

        public override string ToString()
        {
            return $"ID: {Id}; ChargedAccountId: {ChargedAccountId}";
        }
    }
}