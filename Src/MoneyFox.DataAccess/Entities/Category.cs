using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace MoneyFox.DataAccess.Entities
{
    [Table("Categories")]
    internal class Category
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Notes { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.CascadeRead)]
        public List<Payment> Payments { get; set; }
    }
}