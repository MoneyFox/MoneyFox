using PropertyChanged;
using SQLite;

namespace MoneyTracker.Models
{
    [Table("Accounts")]
    [ImplementPropertyChanged]
    public class Account
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Iban { get; set; }

        public double CurrentBalance { get; set; }

        public string Currency { get; set; }

        public string Note { get; set; }

        public int GroupId { get; set; }
    }
}