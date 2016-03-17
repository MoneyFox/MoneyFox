using SQLite.Net.Attributes;

namespace MoneyFox.Foundation.Model
{
    [Table("Categories")]
    public class Category
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}