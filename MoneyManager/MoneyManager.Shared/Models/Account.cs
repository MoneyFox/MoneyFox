using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
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
            CurrencyCulture = ServiceLocator.Current.GetInstance<SettingDataAccess>().CurrencyCulture;
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