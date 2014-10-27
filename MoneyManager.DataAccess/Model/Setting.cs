using PropertyChanged;
using SQLite.Net.Attributes;

namespace MoneyManager.DataAccess.Model
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