using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyFox.DataLayer.Entities
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Note { get; set; }

        public virtual List<PaymentEntity> Payments { get; set; }
        public virtual List<RecurringPaymentEntity> RecurringPayments { get; set; }
    }
}