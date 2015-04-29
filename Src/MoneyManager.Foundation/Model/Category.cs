using PropertyChanged;
using SQLite.Net.Attributes;

namespace MoneyManager.Foundation.Model {
    [ImplementPropertyChanged]
    [Table("Categories")]
    public class Category {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}