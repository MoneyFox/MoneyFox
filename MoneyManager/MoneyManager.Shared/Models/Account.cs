using System.Globalization;
using PropertyChanged;
using SQLite;

namespace MoneyManager.Models
{
    [Table("Accounts")]
    [ImplementPropertyChanged]
    public class Account
    {
        public Account()
        {
            CurrencyCulture = CultureInfo.CurrentCulture.Name;
        }

        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Iban { get; set; }

        public double CurrentBalance { get; set; }

        public string CurrencyCulture { get; set; }

        public string Note { get; set; }

        public int GroupId { get; set; }
    }
}