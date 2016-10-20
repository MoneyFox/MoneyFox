using SQLite;

namespace MoneyFox.DataAccess.DatabaseModels
{
    [Table("Categories")]
    internal class Category
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Notes { get; set; }
    }
}