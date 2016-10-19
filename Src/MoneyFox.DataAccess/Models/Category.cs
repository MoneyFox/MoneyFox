using SQLite.Net.Attributes;

namespace MoneyFox.DataAccess.Models
{
    [Table("Categories")]
    internal class Category
    {
        [PrimaryKey, AutoIncrement, Indexed]
        private int Id { get; set; }

        private string Name { get; set; }
        private string Notes { get; set; }
    }
}