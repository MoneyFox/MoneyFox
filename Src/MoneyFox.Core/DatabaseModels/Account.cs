using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PropertyChanged;

namespace MoneyFox.Core.DatabaseModels
{
    [Table("Accounts")]
    [ImplementPropertyChanged]
    public class Account
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Iban { get; set; }
        public double CurrentBalance { get; set; }
        public string Note { get; set; }
    }
}