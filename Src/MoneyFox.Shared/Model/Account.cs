using PropertyChanged;
using SQLite.Net.Attributes;

namespace MoneyFox.Shared.Model
{
    [ImplementPropertyChanged]
    [Table("Accounts")]
    public class Account
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Iban { get; set; }
        public double CurrentBalance { get; set; }
        public string Note { get; set; }

        public string EndMonthWarning { get; set; }
    }
}