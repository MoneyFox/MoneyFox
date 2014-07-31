using PropertyChanged;
using SQLite;

namespace MoneyTracker.Models
{
    [ImplementPropertyChanged]
    [Table("Setting")]
    public class Setting
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}