using SQLite.Net.Attributes;

namespace MoneyFox.Foundation.Model
{
    [Table("Accounts")]
    public class Account
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Iban { get; set; }
        public double CurrentBalance { get; set; }
        public string Note { get; set; }
    }
}