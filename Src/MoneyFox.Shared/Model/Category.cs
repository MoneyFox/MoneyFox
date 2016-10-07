using PropertyChanged;
using SQLite;

namespace MoneyFox.Shared.Model
{
    [ImplementPropertyChanged]
    [Table("Categories")]
    public class Category
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }

        /// <summary>
        ///     The name of the category
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Additional details about the category
        /// </summary>
        public string Notes { get; set; }
    }
}