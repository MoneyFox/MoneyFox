using PropertyChanged;
using SQLite;

namespace MoneyManager.Models
{
    [ImplementPropertyChanged]
    [Table("Settings")]
    public class Setting
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}