using PropertyChanged;
using SQLite.Net.Attributes;

namespace MoneyManager.DataAccess.Model
{
    [ImplementPropertyChanged]
    [Table("Accounts")]
    public class Country
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }

        public string CurrencyShortName { get; set; }

        public string CurrencyName { get; set; }

        public string CountryName { get; set; }

        public string CountryShortName { get; set; }
    }
}