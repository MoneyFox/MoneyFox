using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyFox.Core.DatabaseModels
{
    [Table("Accounts")]
    public class Account
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Iban { get; set; }
        public double CurrentBalance { get; set; }
        public string Note { get; set; }
    }
}