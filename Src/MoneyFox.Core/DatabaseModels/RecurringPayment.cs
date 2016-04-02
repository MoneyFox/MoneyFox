using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PropertyChanged;

namespace MoneyFox.Core.DatabaseModels
{
    [Table("RecurringPayments")]
    [ImplementPropertyChanged]
    public class RecurringPayment
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsEndless { get; set; }
        public double Amount { get; set; }

        [Required]
        public int Type { get; set; }

        public int Recurrence { get; set; }
        public string Note { get; set; }

        [ForeignKey(nameof(ChargedAccountId))]
        public Account ChargedAccount { get; set; }

        [Required]
        public int ChargedAccountId { get; set; }

        //[ForeignKey(nameof(TargetAccountId))]
        public Account TargetAccount { get; set; }

        public int?TargetAccountId { get; set; }

        //[ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }

        public int? CategoryId { get; set; }

        public override string ToString()
        {
            return $"ID: {Id}; ChargedAccountId: {ChargedAccountId}";
        }
    }
}