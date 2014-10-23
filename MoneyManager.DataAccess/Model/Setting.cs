using PropertyChanged;
using SQLite;
using SQLite.Net.Attributes;

namespace MoneyManager.DataAccess.Model
{
    [ImplementPropertyChanged]
    [Table("Settings")]
    internal class Setting
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}