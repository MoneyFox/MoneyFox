#region

using PropertyChanged;
using SQLite.Net.Attributes;

#endregion

namespace MoneyManager.Foundation.Model {
    [ImplementPropertyChanged]
    [Table("Accounts")]
    public class Account {
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
    }
}