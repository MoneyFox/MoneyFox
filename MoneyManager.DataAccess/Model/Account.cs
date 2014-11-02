using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess.DataAccess;
using PropertyChanged;
using SQLite.Net.Attributes;

namespace MoneyManager.DataAccess.Model
{
    [ImplementPropertyChanged]
    [Table("Accounts")]
    public class Account
    {
        public Account()
        {
            Currency = ServiceLocator.Current.GetInstance<SettingDataAccess>().DefaultCurrency;
        }

        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Iban { get; set; }

        public double CurrentBalanceWithoutExchange { get; set; }

        public double CurrentBalance { get; set; }

        public bool IsExchangeModeActive { get; set; }

        public double ExchangeRatio { get; set; }

        public string Currency { get; set; }

        public string Note { get; set; }

        public int GroupId { get; set; }
    }
}