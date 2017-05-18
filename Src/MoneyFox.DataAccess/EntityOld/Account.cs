using System.ComponentModel.DataAnnotations;

namespace MoneyFox.DataAccess.EntityOld
{
    internal class Account
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Iban { get; set; }
        public double CurrentBalance { get; set; }
        public string Note { get; set; }
        public bool IsOverdrawn { get; set; }
        public bool IsExcluded { get; set; }
    }
}
